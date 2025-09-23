using Project.HR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.HR.Domain.Interfaces
{
    public interface IRoleService
    {
        Task<List<UserRoles>> GetAllRolesAsync();
        Task<UserRoles?> GetRoleByIdAsync(int roleId);
        Task<UserRoles> CreateRoleAsync(UserRoles role);
        Task<UserRoles?> UpdateRoleAsync(int roleId, UserRoles role);
        Task<bool> DeleteRoleAsync(int roleId);
    }
}
