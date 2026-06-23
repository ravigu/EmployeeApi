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
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(
            string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(
                    x => x.Username == username);
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken); 
        }
        public async Task UpdateAsync(User user) 
        { 
            _context.Users.Update(user); 
            await _context.SaveChangesAsync();
        }
    }
}
