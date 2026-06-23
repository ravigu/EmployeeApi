using EmployeeApi.Application.Exceptions;
using EmployeeApi.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Application.Features.Employees.Commands
{
    public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand>
    {
        private readonly IEmployeeRepository _repository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<DeleteEmployeeHandler> _logger;

        public DeleteEmployeeHandler(
            IEmployeeRepository repository, ICacheService cacheService, ILogger<DeleteEmployeeHandler> logger)
        {
            _repository = repository;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task Handle(
            DeleteEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            var employee =
                await _repository.GetByIdAsync(request.Id);

            if (employee == null)
                throw new NotFoundException("Employee not found");


            await _repository.DeleteAsync(employee);

            _logger.LogInformation(
    "Employee {Id} deleted",
    employee.Id);

            await _cacheService.RemoveAsync("employees");
        }
    }
}
