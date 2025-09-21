using Microsoft.EntityFrameworkCore;
using Project.HR.Data.Models;
using Project.HR.Domain.Interfaces;
using Project.HR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.HR.Data.DAL
{
    public class RolesDAL : IRoleService
    {
        private readonly HRDbContext _context;

       

        public RolesDAL(HRDbContext context)
        {
            _context = context;
        }

        public async Task<UserRoles> CreateRoleAsync(UserRoles role)
        {
            await _context
               .UserRoles
               .AddAsync(role);

            await _context
                .SaveChangesAsync();
            return role;
        }

        public Task<bool> DeleteRoleAsync(int roleId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserRoles>> GetAllRolesAsync()
        {
            var connectionString = _context.Database.GetConnectionString();


            return await _context
                .UserRoles
                .ToListAsync();
        }

      

        public Task<UserRoles?> GetRoleByIdAsync(int roleId)
        {
            throw new NotImplementedException();
        }

        public Task<UserRoles?> UpdateRoleAsync(int roleId, UserRoles role)
        {
            throw new NotImplementedException();
        }
    }
}
