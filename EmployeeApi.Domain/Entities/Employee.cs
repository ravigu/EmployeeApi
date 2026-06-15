using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Domain.Entities
{
    public class Employee
    {
        public Guid Id { get; set; } =  Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;


        private Employee()
        {
        }

        public Employee(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public void Update(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
