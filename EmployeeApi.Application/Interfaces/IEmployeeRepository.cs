using EmployeeApi.Application.Common.Models;
using EmployeeApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Application.Interfaces
{
    public interface IEmployeeRepository
    {
        //Task<List<Employee>> GetAllAsync();
        // Task<List<Employee>> GetAllAsync(EmployeeQueryParameters parameters);
        Task<PagedResponse<Employee>> GetAllAsync(EmployeeQueryParameters parameters);
        Task<Employee?> GetByIdAsync(Guid id);

        Task AddAsync(Employee employee);

        Task UpdateAsync(Employee employee);

        Task DeleteAsync(Employee employee);
    }
}
