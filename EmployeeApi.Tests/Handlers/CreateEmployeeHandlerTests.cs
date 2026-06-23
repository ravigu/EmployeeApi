using EmployeeApi.Application.Features.Employees.Commands;
using EmployeeApi.Application.Interfaces;
using EmployeeApi.Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Tests.Handlers
{
    /*public class CreateEmployeeHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Create_Employee()
        {
            // Arrange

            var repository =new Mock<IEmployeeRepository>();

            var handler =new CreateEmployeeHandler(repository.Object);

            var command = new CreateEmployeeCommand
                (
                    "Ravi",
                    "ravi@gmail.com"
                );

            // Act

            var result =await handler.Handle(command, CancellationToken.None);

            // Assert

            result.Should().NotBe(Guid.Empty);

            repository.Verify(x => x.AddAsync(  It.IsAny<Employee>()),Times.Once);
        }
    }*/
}
