using EmployeeApi.Application.DTOs;
using EmployeeApi.Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController( IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var token = await _mediator.Send( new LoginCommand( request.Username, request.Password));

            return Ok(new
            {
                Token = token
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken( RefreshTokenDto request)
        {
            var result =  await _mediator.Send(
                    new RefreshTokenCommand(
                        request.RefreshToken));

            return Ok(result);
        }
    }
}
