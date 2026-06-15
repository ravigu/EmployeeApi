using EmployeeApi.Application.Interfaces;
using MediatR;
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

        public DeleteEmployeeHandler(
            IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(
            DeleteEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            var employee =
                await _repository.GetByIdAsync(request.Id);

            if (employee == null)
                throw new Exception("Employee not found");

            await _repository.DeleteAsync(employee);
        }
    }
}
