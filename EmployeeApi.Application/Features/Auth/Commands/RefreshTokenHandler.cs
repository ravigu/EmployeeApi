using EmployeeApi.Application.DTOs;
using EmployeeApi.Application.Exceptions;
using EmployeeApi.Application.Interfaces;
using MediatR;

namespace EmployeeApi.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenHandler: IRequestHandler<  RefreshTokenCommand,  TokenResponseDto>
{
    private readonly IUserRepository _userRepository;

    private readonly IJwtService _jwtService;

    public RefreshTokenHandler(
        IUserRepository userRepository,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<TokenResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user =await _userRepository .GetByRefreshTokenAsync( request.RefreshToken);

        if (user == null)
            throw new UnauthorizedException("Invalid refresh token");

        if (user.RefreshTokenExpiryTime <DateTime.UtcNow)
        {
            throw new UnauthorizedException( "Refresh token expired");
        }

        var accessToken = _jwtService.GenerateToken(
                    user.Id,
                    user.Username,
                    user.Role); 

        var newRefreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;

        user.RefreshTokenExpiryTime =DateTime.UtcNow.AddDays(7);

        await _userRepository.UpdateAsync( user);

        return new TokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        };
    }
}