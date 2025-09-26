using Project.HR.Domain.DTOs;
using Project.HR.Domain.Models;

namespace Project.HR.Domain.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<DepartmentDTO?>> GetAllDepartmentsAsync();
        
        Task<Department> CreateDepartmentAsync(Department department);
        Task<Department?> GetDepartmentByNameAsync(string departmentName);
        Task<DepartmentDTO?> GetDepartmentByIdAsync(int id);
        Task<bool> DeleteDepartmentAsync(int id);
        Task<Department?> UpdateDepartmentAsync(int id, Department department);
    }
}
