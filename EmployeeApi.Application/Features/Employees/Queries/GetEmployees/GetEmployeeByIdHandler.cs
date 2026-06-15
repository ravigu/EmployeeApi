using AutoMapper;
using EmployeeApi.Application.DTOs;
using EmployeeApi.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Application.Features.Employees.Queries.GetEmployees
{
    public class GetEmployeeByIdHandler : IRequestHandler<  GetEmployeeByIdQuery,  EmployeeDto>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public GetEmployeeByIdHandler(
            IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request,CancellationToken cancellationToken)
        {
            var employee =  await _repository.GetByIdAsync(request.Id);

            if (employee == null)
                throw new Exception("Employee not found");

            return _mapper.Map<EmployeeDto>( employee);
        }
    }
}
