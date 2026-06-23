using Asp.Versioning;
using EmployeeApi.Application.Common;
using EmployeeApi.Application.Common.Models;
using EmployeeApi.Application.DTOs;
using EmployeeApi.Application.Features.Employees.Commands;
using EmployeeApi.Application.Features.Employees.Queries.GetEmployees;
using EmployeeApi.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace EmployeeApi.Api.Controller
{
    [Authorize]
    [ApiController]
    [EnableRateLimiting("fixed")]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

      
        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    var result =
        //        await _service.GetAllAsync();

        //    return Ok(result);
        //}

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] EmployeeQueryParameters parameters)
        {
            var result = await _mediator.Send( new GetEmployeesQuery(parameters));

            return Ok( new ApiResponse<PagedResponse<EmployeeDto>>
                (
                    true,
                    "Employees fetched successfully",
                    result
                ));
        }
        /*
                [HttpGet("{id:guid}")]
                public async Task<EmployeeDto?> GetByIdAsync(Guid id)
                {
                    var employee = await _service.GetByIdAsync(id);

                    if (employee == null)
                        throw new Exception("Employee not found");

                    return new EmployeeDto
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        Email = employee.Email
                    };
                }*/

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var employee =await _mediator.Send(new GetEmployeeByIdQuery(id));

            return Ok(employee);
        }
        /*
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateEmployeeDto request)
        {
            await _service.CreateAsync(request);

            return Ok();
        }*/

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateE(CreateEmployeeDto request)
        {
            var id = await _mediator.Send(
                new CreateEmployeeCommand(
                    request.Name,
                    request.Email));

            return Ok(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(  Guid id,UpdateEmployeeDto request)
        {
            //return Ok( await _service.GetByIdAsync(id));

            await _mediator.Send( new UpdateEmployeeCommand(id,request.Name, request.Email));

            return NoContent();
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete( Guid id)
        {
            /*await _service.DeleteAsync(id);

            return NoContent();*/
            await _mediator.Send( new DeleteEmployeeCommand(id));

            return NoContent();
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Username = User.Identity?.Name,
                Role = User.FindFirst(ClaimTypes.Role)?.Value
            });
        }
    }


}
