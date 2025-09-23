using Project.HR.Domain.Models;

namespace Project.HR.Domain.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAllDepartmentsAsync();
        
        Task<Department> CreateDepartmentAsync(Department department);
        Task<Department?> GetDepartmentByNameAsync(string departmentName);
        Task<bool> DeleteDepartmentAsync(string departmentName);
        Task<Department?> UpdateDepartmentAsync(int id, Department department);
    }
}
