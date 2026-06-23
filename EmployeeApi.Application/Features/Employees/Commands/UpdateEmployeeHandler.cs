using EmployeeApi.Application.Interfaces;
using EmployeeApi.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Application.Features.Employees.Commands
{
    public class UpdateEmployeeHandler: IRequestHandler<UpdateEmployeeCommand>
    {
        private readonly IEmployeeRepository _repository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<UpdateEmployeeHandler> _logger;
        public UpdateEmployeeHandler(
            IEmployeeRepository repository, ICacheService cacheService, ILogger<UpdateEmployeeHandler> logger)
        {
            _repository = repository;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task Handle(
            UpdateEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            var employee =
                await _repository.GetByIdAsync(request.Id);

            if (employee == null)
                throw new NotFoundException("Employee not found");

            employee.Name = request.Name;
            employee.Email = request.Email;

            await _repository.UpdateAsync(employee);
            _logger.LogInformation(
    "Employee {Id} updated",
    employee.Id);
            await _cacheService.RemoveAsync("employees");
        }
    }
}
