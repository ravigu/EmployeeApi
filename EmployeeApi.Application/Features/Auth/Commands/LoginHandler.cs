using EmployeeApi.Application.DTOs;
using EmployeeApi.Application.Exceptions;
using EmployeeApi.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Application.Features.Auth.Commands
{
    public class LoginHandler: IRequestHandler<LoginCommand, TokenResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly ILogger<LoginHandler> _logger;

        public LoginHandler(
            IUserRepository userRepository,
            IJwtService jwtService,
            ILogger<LoginHandler> logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<TokenResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user =await _userRepository.GetByUsernameAsync( request.Username);

            if (user == null)
            {
                _logger.LogWarning("Login failed for username {Username}", request.Username);
              
                    throw new UnauthorizedException("Invalid username or password");

            }

            var validPassword = BCrypt.Net.BCrypt.Verify(  request.Password,user.PasswordHash);

            if (!validPassword)
            {
                _logger.LogWarning("Invalid password for user {Username}", request.Username);
                throw new UnauthorizedException("Invalid username or password");


            }

            var accessToken =_jwtService.GenerateToken(
                            user.Id,
                            user.Username,
                            user.Role);

            var refreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            user.RefreshTokenExpiryTime =DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateAsync(user);

            _logger.LogInformation( "User {Username} logged in successfully", request.Username);

            return new TokenResponseDto { AccessToken = accessToken, RefreshToken = refreshToken };


        }
    }
}
