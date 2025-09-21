using Project.HR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.HR.Domain
{
    internal interface IEmployeeService
    {
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<Employee?> GetEmployeeByEmployeeIdAsync(string employeeId);
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<List<Employee>> GetEmployeesByDepartmentAsync(int departmentId);
        Task<List<Employee>> GetDirectReportsAsync(int managerId);
        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<Employee> UpdateEmployeeAsync(Employee employee);
        Task<bool> TerminateEmployeeAsync(int employeeId, DateTime terminationDate, string reason);
    }
}
