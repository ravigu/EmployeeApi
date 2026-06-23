using EmployeeApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Infrastructure.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> option):base( option)
        {
            
        }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<User> Users =>Set<User>();
    }
}
