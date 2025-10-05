using Microsoft.EntityFrameworkCore;
using Project.HR.Data.Models;
using Project.HR.Domain.Helpers;
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
        // In your endpoint or DAL, before SaveChangesAsync:
        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            // Validate all foreign keys exist
            var roleExists = await _context.UserRoles.AnyAsync(r => r.RoleId == employee.RoleId);
            if (!roleExists)
                throw new InvalidOperationException($"RoleId {employee.RoleId} does not exist in UserRoles table");

           

           

            if (employee.ManagerId.HasValue)
            {
                var managerExists = await _context.Employees.AnyAsync(e => e.UserId == employee.ManagerId.Value);
                if (!managerExists)
                    throw new InvalidOperationException($"ManagerId {employee.ManagerId} does not exist in Employees table");
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
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

        public async Task<Employee?> UpdateEmployeeAsync(int id, Employee employee)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                LogErrorHelper.LogError($"Updating employee with ID: {id}", null, LogErrorHelper.ErrorLevel.Info);

                var existingEmployee = await _context.Employees
                    .Include(e => e.Department)  // Include related data if needed
                    .Include(e => e.Position)
                    .FirstOrDefaultAsync(e => e.UserId == id);

                if (existingEmployee == null)
                {
                    LogErrorHelper.LogError($"Employee with ID {id} not found for update", null, LogErrorHelper.ErrorLevel.Warn);
                    return null;
                }

                // Validation: Check if email is unique (excluding current employee)
                if (await _context.Employees.AnyAsync(e => e.Email == employee.Email && e.UserId != id))
                {
                    throw new InvalidOperationException($"Email {employee.Email} is already in use by another employee.");
                }

                // Validation: Check if department exists
                if (!await _context.Departments.AnyAsync(d => d.Id == employee.DepartmentId))
                {
                    throw new InvalidOperationException($"Department with ID {employee.DepartmentId} does not exist.");
                }

                // Validation: Check if manager exists (if specified)
                if (employee.ManagerId.HasValue &&
                    !await _context.Employees.AnyAsync(e => e.UserId == employee.ManagerId.Value))
                {
                    throw new InvalidOperationException($"Manager with ID {employee.ManagerId} does not exist.");
                }

                // Update properties
                existingEmployee.FirstName = employee.FirstName;
                existingEmployee.LastName = employee.LastName;
                existingEmployee.Email = employee.Email;
                existingEmployee.PhoneNumber = employee.PhoneNumber;
                existingEmployee.Address = employee.Address;
                existingEmployee.City = employee.City;
                existingEmployee.State = employee.State;
                existingEmployee.ZipCode = employee.ZipCode;
                existingEmployee.Country = employee.Country;
                existingEmployee.DateOfBirth = employee.DateOfBirth;
                existingEmployee.DepartmentId = employee.DepartmentId;
                existingEmployee.ManagerId = employee.ManagerId;
                existingEmployee.PositionId = employee.PositionId;
                existingEmployee.RoleId = employee.RoleId;
                existingEmployee.Status = employee.Status;

                // Update timestamp if you have one
                // existingEmployee.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                LogErrorHelper.LogError($"Employee with ID {id} updated successfully", null, LogErrorHelper.ErrorLevel.Info);
                return existingEmployee;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                LogErrorHelper.LogError($"Failed to update employee with ID {id}", ex);
                throw;
            }
        }
    }
}
