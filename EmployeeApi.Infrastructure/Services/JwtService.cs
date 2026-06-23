using EmployeeApi.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(
            Guid userId,
            string username,
            string role)
        {
            var claims = new[]
            {
            new Claim(
                ClaimTypes.NameIdentifier,
                userId.ToString()),

            new Claim(
                ClaimTypes.Name,
                username),

            new Claim(
                ClaimTypes.Role,
                role)
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token =
                new JwtSecurityToken(
                    issuer:
                        _configuration["Jwt:Issuer"],

                    audience:
                        _configuration["Jwt:Audience"],

                    claims:
                        claims,

                    expires:
                        DateTime.UtcNow.AddHours(1),

                    signingCredentials:
                        credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            using var rng =
                RandomNumberGenerator.Create();

            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(
                randomNumber);
        }
    }
}
