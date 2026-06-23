using EmployeeApi.Application.Common.Models;
using EmployeeApi.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Application.Features.Employees.Queries.GetEmployees
{
    public record GetEmployeesQuery( EmployeeQueryParameters Parameters) : IRequest<PagedResponse<EmployeeDto>>;


}
