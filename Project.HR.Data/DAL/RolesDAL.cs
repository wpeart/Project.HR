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

        public async Task<bool> DeleteRoleAsync(int roleId)
        {
            _context.UserRoles.Remove(new UserRoles { RoleId = roleId });
            return await _context.SaveChangesAsync().ContinueWith(t => t.Result > 0);

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

        public async Task<UserRoles> UpdateRoleAsync(int roleId, UserRoles role)
        {
            _context.UserRoles.Update(role);
            await _context.SaveChangesAsync();

            return role;
        }
    }
}
