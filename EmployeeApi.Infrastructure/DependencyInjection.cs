using EmployeeApi.Application.Interfaces;
using EmployeeApi.Infrastructure.Data;
using EmployeeApi.Infrastructure.Repositories;
using EmployeeApi.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services , IConfiguration configuration)
        {

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<  IEmployeeRepository,  EmployeeRepository>();

            services.AddScoped<  IJwtService,JwtService>();
            services.AddScoped< IUserRepository,UserRepository>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration =
                    configuration["Redis:ConnectionString"];

                options.InstanceName = "EmployeeApi:";
            });

            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<IEmailService, EmailService>();
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            return services;
        }
    }
    }
