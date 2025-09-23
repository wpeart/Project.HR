using Project.HR.Domain.DTOs;
using Project.HR.Domain.Helpers;
using Project.HR.Domain.Interfaces;
using Project.HR.Domain.Models;

namespace Project.HR.WebAPI.Endpoints
{
    public static class EmployeeEndpoint
    {
        public static void MapEmployeeEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/employees").WithTags("Employee");
            group.MapGet("/", async (IEmployeeService employeeService) =>
            {
                try
                {
                    var employees = await employeeService.GetAllEmployeesAsync();
                    return Results.Ok(employees);
                }
                catch (Exception ex)
                {
                    LogErrorHelper.LogError("Error fetching all employees", ex, LogErrorHelper.ErrorLevel.Error);
                    return Results.Problem("An error occurred while processing your request.");
                }
            })
            .WithName("GetAllEmployees")
            .Produces<List<Employee>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);
            group.MapGet("/{id:int}", async (int id, IEmployeeService employeeService) =>
            {
                try
                {
                    var employee = await employeeService.GetEmployeeByIdAsync(id);
                    return employee is not null ? Results.Ok(employee) : Results.NotFound();
                }
                catch (Exception ex)
                {
                    LogErrorHelper.LogError($"Error fetching employee with ID {id}", ex, LogErrorHelper.ErrorLevel.Error);
                    return Results.Problem("An error occurred while processing your request.");
                }
            })
            .WithName("GetEmployeeById")
            .Produces<Employee>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
            group.MapPost("/", async (EmployeeDTO employeeDto, IEmployeeService employeeService) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(employeeDto.FirstName) || string.IsNullOrWhiteSpace(employeeDto.LastName) || string.IsNullOrWhiteSpace(employeeDto.Email))
                    {
                        return Results.BadRequest("First name, last name, and email are required.");
                    }

                    Employee employee = new Employee
                    {
                        FirstName = employeeDto.FirstName,
                        LastName = employeeDto.LastName,
                        Email = employeeDto.Email,
                        PhoneNumber = employeeDto.PhoneNumber,
                        HireDate = employeeDto.HireDate,
                        Address = employeeDto.Address,
                        City = employeeDto.City,
                        State = employeeDto.State,
                        ZipCode = employeeDto.ZipCode,
                        Country = employeeDto.Country,
                        DateOfBirth = employeeDto.DateOfBirth,

                        DepartmentId = employeeDto.DepartmentId,
                        ManagerId = employeeDto.ManagerId,
                        Password = employeeDto.Password,
                        Status = employeeDto.Status,
                         Gender= employeeDto.Gender,
                          MaritalStatus= employeeDto.MaritalStatus
                          , MiddleName= employeeDto.MiddleName,
                           PositionId= employeeDto.PositionId
                           , RoleId = employeeDto.RoleId
                           , UserName = employeeDto.UserName
                             
                    };
                    var createdEmployee = await employeeService.CreateEmployeeAsync(employee);
                    return Results.Created($"/api/employees/{createdEmployee.UserId}", createdEmployee);
                }
                catch (Exception ex)
                {
                    LogErrorHelper.LogError("Error creating new employee", ex, LogErrorHelper.ErrorLevel.Error);
                    return Results.Problem("An error occurred while processing your request.");
                }
            })
            .WithName("CreateEmployee")
            .Accepts<Employee>("application/json")
            .Produces<Employee>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapPut("/{id:int}", async (int id, EmployeeDTO employeeDto, IEmployeeService employeeService) =>
            {
                try
                {
                    // Validation
                    if (string.IsNullOrWhiteSpace(employeeDto.FirstName))
                    {
                        return Results.BadRequest("First name is required.");
                    }

                    if (string.IsNullOrWhiteSpace(employeeDto.LastName))
                    {
                        return Results.BadRequest("Last name is required.");
                    }

                    if (string.IsNullOrWhiteSpace(employeeDto.Email))
                    {
                        return Results.BadRequest("Email is required.");
                    }

                    // Basic email format validation
                    if (!employeeDto.Email.Contains("@"))
                    {
                        return Results.BadRequest("Invalid email format.");
                    }

                    if (employeeDto.DepartmentId <= 0)
                    {
                        return Results.BadRequest("Valid department ID is required.");
                    }

                    // Optional: Validate hire date is not in the future
                    if (employeeDto.HireDate > DateTime.Now)
                    {
                        return Results.BadRequest("Hire date cannot be in the future.");
                    }

                    LogErrorHelper.LogError($"Updating employee with ID: {id}", null, LogErrorHelper.ErrorLevel.Info);

                    Employee employee = new Employee
                    {
                        UserId = id,
                        FirstName = employeeDto.FirstName.Trim(),
                        LastName = employeeDto.LastName.Trim(),
                        Email = employeeDto.Email.Trim().ToLowerInvariant(),
                        PhoneNumber = employeeDto.PhoneNumber?.Trim(),
                        HireDate = employeeDto.HireDate,
                        Address = employeeDto.Address?.Trim(),
                        City = employeeDto.City?.Trim(),
                        State = employeeDto.State?.Trim(),
                        ZipCode = employeeDto.ZipCode?.Trim(),
                        Country = employeeDto.Country?.Trim(),
                        DateOfBirth = employeeDto.DateOfBirth,
                        DepartmentId = employeeDto.DepartmentId,
                        ManagerId = employeeDto.ManagerId,
                        PositionId = employeeDto.PositionId,
                        RoleId = employeeDto.RoleId,
                        Status = employeeDto.Status
                    };


                    var updatedEmployee = await employeeService.UpdateEmployeeAsync(id, employee);


                    return updatedEmployee is not null
                        ? Results.Ok(updatedEmployee)
                        : Results.NotFound($"Employee with ID {id} not found.");
                }
                catch (Exception ex)
                {

                    LogErrorHelper.LogError("Error updating employee", ex);
                    return Results.Problem("An error occurred while updating the employee.");
                }
            })
 .WithName("UpdateEmployee")
 .Accepts<EmployeeDTO>("application/json")
 .Produces<Employee>(StatusCodes.Status200OK)
 .Produces(StatusCodes.Status400BadRequest)
 .Produces(StatusCodes.Status404NotFound)
 .ProducesProblem(StatusCodes.Status500InternalServerError);
        }
    }
}
