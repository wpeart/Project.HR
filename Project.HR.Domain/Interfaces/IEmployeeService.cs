using Project.HR.Domain.Models;

namespace Project.HR.Domain.Interfaces
{
    public interface IEmployeeService
    {
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<Employee?> GetEmployeeByEmployeeIdAsync(string employeeId);
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<List<Employee>> GetEmployeesByDepartmentAsync(int departmentId);
        Task<List<Employee>> GetDirectReportsAsync(int managerId);
        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<Employee> UpdateEmployeeAsync(int id, Employee employee);
        Task<bool> TerminateEmployeeAsync(int employeeId, DateTime terminationDate, string reason);
    }
}
