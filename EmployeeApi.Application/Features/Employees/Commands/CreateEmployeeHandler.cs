using EmployeeApi.Application.Interfaces;
using EmployeeApi.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Application.Features.Employees.Commands
{
    public class CreateEmployeeHandler : IRequestHandler< CreateEmployeeCommand, Guid>
    {
        private readonly IEmployeeRepository _repository;

        public CreateEmployeeHandler( IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateEmployeeCommand request,CancellationToken cancellationToken)
        {
            var employee = new Employee(
                request.Name,
                request.Email);

            await _repository.AddAsync(employee);

            return employee.Id;
        }
    }
}
