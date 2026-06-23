using Asp.Versioning;
using EmployeeApi.Api.Middlewares;
using EmployeeApi.Application;
using EmployeeApi.Application.Interfaces;
using EmployeeApi.Infrastructure;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;



Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day)
    .WriteTo.Seq("http://seq")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services
builder.Services.AddControllers();

builder.Services
.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);

    options.AssumeDefaultVersionWhenUnspecified = true;

    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";

    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(
        "fixed",
        opt =>
        {
            opt.PermitLimit = 5;
            opt.Window = TimeSpan.FromMinutes(1);

            opt.QueueProcessingOrder =
                QueueProcessingOrder.OldestFirst;

            opt.QueueLimit = 0;
        });

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;

        await context.HttpContext.Response.WriteAsJsonAsync(
            new
            {
                Message = "Too many requests. Please try again later."
            },
            cancellationToken: token);
    };
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
    "Bearer",
    new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Token"
    });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
        {
            new OpenApiSecurityScheme
            {
                Reference =
                    new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
            },
            Array.Empty<string>()
        }
        });

    options.SwaggerDoc(
    "v1",
    new OpenApiInfo
    {
        Title = "EmployeeApi V1",
        Version = "v1"
    });

    options.SwaggerDoc(
        "v2",
        new OpenApiInfo
        {
            Title = "EmployeeApi V2",
            Version = "v2"
        });

});

builder.Services.AddApplication();

builder.Services.AddInfrastructure(
builder.Configuration);

builder.Services
.AddAuthentication(
JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
    new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

        ValidAudience =
                    builder.Configuration["Jwt:Audience"],

        IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!))
    };
});


builder.Services.AddHealthChecks()
    .AddNpgSql(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "postgres")
    .AddRedis(
        builder.Configuration["Redis:ConnectionString"]!,
        name: "redis");

builder.Services.AddHangfire(config =>
{
    config
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UsePostgreSqlStorage(
            builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHangfireServer();

builder.Services.AddAuthorization();
Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));
var app = builder.Build();

// Middleware

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        "EmployeeApi V1");

    options.SwaggerEndpoint(
        "/swagger/v2/swagger.json",
        "EmployeeApi V2");
});

app.UseSerilogRequestLogging();

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseRateLimiter();
// Hangfire Dashboard
app.UseHangfireDashboard("/hangfire",
    new DashboardOptions
    {
        Authorization = Array.Empty<IDashboardAuthorizationFilter>()
    });
// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(x => new
            {
                service = x.Key,
                status = x.Value.Status.ToString()
            })
        });

        await context.Response.WriteAsync(result);
    }
});
// Temporary endpoint to generate password hash
app.MapGet("/hash", () =>
{
    return BCrypt.Net.BCrypt.HashPassword("ravi@123");
});

app.MapGet("/job", () =>
{
    BackgroundJob.Enqueue(
        () => Console.WriteLine("Background job executed"));

    return "Job Created";
});


app.MapGet("/send-email", (IBackgroundJobClient backgroundJobClient) =>
{
    backgroundJobClient.Enqueue<IEmailService>(
        x => x.SendWelcomeEmail("ravi.jeetendra@gmail.com"));

    return "Email Job Added";
});


RecurringJob.AddOrUpdate<IEmailService>(
    "daily-email-job",
    x => x.SendWelcomeEmail("ravi.jeetendra@gmail.com"),
    Cron.Minutely);

app.Run();