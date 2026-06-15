using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Application.Services
{

    public class NotFoundException : Exception
    {
        public NotFoundException(
            string message)
            : base(message)
        {
        }
    }
}
