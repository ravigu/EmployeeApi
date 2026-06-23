using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId, string username, string role);
        string GenerateRefreshToken();
    }
}
