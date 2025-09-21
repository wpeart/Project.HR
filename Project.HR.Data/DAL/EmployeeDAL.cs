using Microsoft.EntityFrameworkCore;
using Project.HR.Data.Models;
using Project.HR.Domain.Interfaces;
using Project.HR.Domain.Models;

namespace Project.HR.Data.DAL
{
    public class EmployeeDAL : IEmployeeService
    {
        private readonly HRDbContext _context;

        public EmployeeDAL(HRDbContext context)
        {
            _context = context;
        }
        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            await _context
                .Employees
                .AddAsync(employee);
            await _context
                .SaveChangesAsync();

            return employee;

        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.AsNoTracking().ToListAsync();
        }

        public async Task<List<Employee>> GetDirectReportsAsync(int managerId)
        {
            return await _context.Employees
                 .AsNoTracking()
                 .Where(e => e.ManagerId == managerId)
                 .ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByEmployeeIdAsync(int employeeId)
        {
            return await _context.Employees
                 .AsNoTracking()
                 .Include(e => e.Department)
                 .Include(e => e.Position)
                 .Include(e => e.Manager)
                 .Include(e => e.DirectReports)
                 .FirstOrDefaultAsync(e => e.UserId == employeeId);
        }

        public async Task<Employee?> GetEmployeeByEmployeeIdAsync(string userName)
        {
            return await _context.Employees
                 .AsNoTracking()
                 .Include(e => e.Department)
                 .Include(e => e.Position)
                 .Include(e => e.Manager)
                 .Include(e => e.DirectReports)
                 .FirstOrDefaultAsync(e => e.UserName == userName);
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
                  .AsNoTracking()
                  .Include(e => e.Department)
                  .Include(e => e.Position)
                  .Include(e => e.Manager)
                  .Include(e => e.DirectReports)
                  .FirstOrDefaultAsync(e => e.UserId == id);
        }

        public async Task<List<Employee>> GetEmployeesByDepartmentAsync(int departmentId)
        {
            return await _context.Employees
                 .AsNoTracking()
                 .Where(e => e.DepartmentId == departmentId)
                 .ToListAsync();
        }

        public async Task<bool> TerminateEmployeeAsync(int employeeId, DateTime terminationDate, string reason)
        {
            return await _context.Employees
                 .Where(e => e.UserId == employeeId)
                 .ExecuteUpdateAsync(e => e
                     .SetProperty(emp => emp.TerminationDate, terminationDate)
                     .SetProperty(emp => emp.Status, Domain.Enums.EmployeeStatus.Inactive)
                 ) > 0;
        }

        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            _context
                .Employees
               .Update(employee);
            await _context
                .SaveChangesAsync();
            return employee;


        }
    }
}
