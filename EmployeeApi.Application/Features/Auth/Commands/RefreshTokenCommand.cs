using EmployeeApi.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Application.Features.Auth.Commands
{
    public record RefreshTokenCommand(
     string RefreshToken)
     : IRequest<TokenResponseDto>;
}
