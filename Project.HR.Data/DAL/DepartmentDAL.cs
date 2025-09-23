using Microsoft.EntityFrameworkCore;
using Project.HR.Data.Models;
using Project.HR.Domain.Interfaces;
using Project.HR.Domain.Models;

namespace Project.HR.Data.DAL
{
    public class DepartmentDAL : IDepartmentService
    {
        private readonly HRDbContext _context;

        public DepartmentDAL(HRDbContext context)
        {
            _context = context;
        }

        public async Task<Department> CreateDepartmentAsync(Department department)
        {
            await _context
                .Departments
                .AddAsync(department);
            await _context.SaveChangesAsync();

            return department;
        }

        public async Task<bool> DeleteDepartmentAsync(string departmentName)
        {
            await _context.Departments
                 .Where(d => d.Name == departmentName)
                 .ExecuteDeleteAsync();
            return true;
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            return await _context.Departments
                  .AsNoTracking()
                  .ToListAsync();
        }

        public async Task<Department?> GetDepartmentByNameAsync(string departmentName)
        {
            return await _context.Departments
                     .AsNoTracking()
                     .FirstOrDefaultAsync(d => d.Name == departmentName);
        }

        public async Task<Department?> UpdateDepartmentAsync(int id, Department department)
        {
            await _context.Departments
                 .Where(d => d.Id == id)
                 .ExecuteUpdateAsync(d => d
                     .SetProperty(p => p.Name, department.Name)
                     .SetProperty(p => p.Description, department.Description)
                     .SetProperty(p => p.Code, department.Code)
                     .SetProperty(p => p.Budget, department.Budget)
                        .SetProperty(p => p.ParentDepartmentId, department.ParentDepartmentId)
                 );
            return department;
        }
    }
}
