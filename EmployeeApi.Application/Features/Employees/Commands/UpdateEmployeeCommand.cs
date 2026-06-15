using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Application.Features.Employees.Commands
{
    public record UpdateEmployeeCommand( Guid Id,  string Name, string Email) : IRequest;
}
