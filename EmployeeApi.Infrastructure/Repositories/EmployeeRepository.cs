using EmployeeApi.Application.Common.Models;
using EmployeeApi.Application.Interfaces;
using EmployeeApi.Domain.Entities;
using EmployeeApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(
            AppDbContext context)
        {
            _context = context;
        }

        /* public async Task<List<Employee>> GetAllAsync()
         {
             return await _context.Employees.ToListAsync();
         }
 */
        /* public async Task<List<Employee>> GetAllAsync(EmployeeQueryParameters parameters)
         {
             IQueryable<Employee> query = _context.Employees;

             // Search
             if (!string.IsNullOrWhiteSpace(parameters.Search))
             {
                 query = query.Where(x =>x.Name.ToLower().Contains(parameters.Search.ToLower()));
             }

             // Sorting
             query = parameters.Descending?query.OrderByDescending(x => x.Name): query.OrderBy(x => x.Name);

             // Pagination
             query = query.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize);

             return await query.ToListAsync();
         }*/

        public async Task<PagedResponse<Employee>> GetAllAsync(
    EmployeeQueryParameters parameters)
        {
            IQueryable<Employee> query = _context.Employees;

            // Search
            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                query = query.Where(x =>
                    x.Name.ToLower()
                     .Contains(parameters.Search.ToLower()));
            }

            // Sorting
            query = parameters.Descending
                ? query.OrderByDescending(x => x.Name)
                : query.OrderBy(x => x.Name);

            // Total records before paging
            var totalRecords = await query.CountAsync();

            // Paging
            var employees = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedResponse<Employee>
            {
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(
                    totalRecords / (double)parameters.PageSize),

                Items = employees
            };
        }
        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Employee employee)
        {
            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();
        }
    }
}
