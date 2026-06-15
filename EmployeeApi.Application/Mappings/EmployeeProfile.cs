using AutoMapper;
using EmployeeApi.Application.DTOs;
using EmployeeApi.Application.Features.Employees.Commands;
using EmployeeApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EmployeeApi.Application.Mappings
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>();

            CreateMap<CreateEmployeeCommand, Employee>();

            CreateMap<UpdateEmployeeCommand, Employee>();
        }
    }
}
