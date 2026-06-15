using AutoMapper;
using EmployeeApi.Application.DTOs;
using EmployeeApi.Application.Interfaces;
using EmployeeApi.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Application.Features.Employees.Queries.GetEmployees
{
    public class GetEmployeesHandler: IRequestHandler< GetEmployeesQuery, List<EmployeeDto>>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;


        public GetEmployeesHandler(
            IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDto>> Handle(
            GetEmployeesQuery request,
            CancellationToken cancellationToken)
        {
            var employees = await _repository.GetAllAsync();

            return _mapper.Map<List<EmployeeDto>>( employees);
        }
    }
}
