using EmployeeApi.Application.Features.Auth.Commands;
using EmployeeApi.Application.Interfaces;
using EmployeeApi.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ICacheService _cacheService;
        private readonly ILogger<CreateEmployeeHandler> _logger;

        public CreateEmployeeHandler( IEmployeeRepository repository , ICacheService cacheService , ILogger<CreateEmployeeHandler> logger)
        {
            _repository = repository;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateEmployeeCommand request,CancellationToken cancellationToken)
        {
            var employee = new Employee(
                request.Name,
                request.Email);

            await _repository.AddAsync(employee);
            _logger.LogInformation("Employee {Name} created successfully", employee.Name);

            await _cacheService.RemoveAsync("employees");
            return employee.Id;
        }
    }
}
