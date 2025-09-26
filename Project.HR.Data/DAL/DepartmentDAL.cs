using Microsoft.EntityFrameworkCore;
using Project.HR.Data.Models;
using Project.HR.Domain.DTOs;
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

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            await _context.Departments
                 .Where(d => d.Id == id)
                 .ExecuteDeleteAsync();
            return true;
        }

        public async Task<List<DepartmentDTO?>> GetAllDepartmentsAsync()
        {
            var department = await _context.Departments
                  .Select(d => new DepartmentDTO
                  {
                      Id = d.Id,
                      Name = d.Name,
                      Code = d.Code,
                      Description = d.Description,
                      ParentDepartmentId = d.ParentDepartmentId,
                      Budget = d.Budget,
                      ParentDepartmentName = d.ParentDepartment != null ? d.ParentDepartment.Name : null
                  })
        .ToListAsync();
            return department;
        }

        public async Task<DepartmentDTO?> GetDepartmentByIdAsync(int id)
        {
            var department = await _context.Departments
                .AsNoTracking()
                .Include(d => d.ParentDepartment)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null) return null;

            return new DepartmentDTO
            {
                Id = department.Id,
                Name = department.Name,
                Code = department.Code,
                Description = department.Description,
                ParentDepartmentId = department.ParentDepartmentId,
                Budget = department.Budget,
                ParentDepartmentName = department.ParentDepartment?.Name
            };
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
