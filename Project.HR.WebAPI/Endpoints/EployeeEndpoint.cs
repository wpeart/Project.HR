using Project.HR.Domain.DTOs;
using Project.HR.Domain.Enums;
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
                    // Validation
                    if (string.IsNullOrWhiteSpace(employeeDto.FirstName))
                    {
                        LogErrorHelper.LogError("First name is required.", null, LogErrorHelper.ErrorLevel.Warn);
                        return Results.BadRequest("First name is required.");
                    }

                    if (string.IsNullOrWhiteSpace(employeeDto.LastName))
                    {
                        LogErrorHelper.LogError("Last name is required.", null, LogErrorHelper.ErrorLevel.Warn);
                        return Results.BadRequest("Last name is required.");
                    }

                    if (string.IsNullOrWhiteSpace(employeeDto.Email))
                    {
                        LogErrorHelper.LogError("Email is required.", null, LogErrorHelper.ErrorLevel.Warn);
                        return Results.BadRequest("Email is required.");
                    }

                    // Basic email format validation
                    if (!employeeDto.Email.Contains("@"))
                    {
                        LogErrorHelper.LogError("Invalid email format.", null, LogErrorHelper.ErrorLevel.Warn);
                        return Results.BadRequest("Invalid email format.");
                    }

                    if (employeeDto.DepartmentId <= 0)
                    {
                        LogErrorHelper.LogError("Valid department ID is required.", null, LogErrorHelper.ErrorLevel.Warn);
                        return Results.BadRequest("Valid department ID is required.");
                    }

                    // Optional: Validate hire date is not in the future
                    if (employeeDto.HireDate > DateTime.Now)
                    {
                        LogErrorHelper.LogError("Hire date cannot be in the future.", null, LogErrorHelper.ErrorLevel.Warn);
                        return Results.BadRequest("Hire date cannot be in the future.");
                    }
                    LogErrorHelper.LogError("Creating new employee", null, LogErrorHelper.ErrorLevel.Info);
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
                        Gender = employeeDto.Gender,
                        MaritalStatus = employeeDto.MaritalStatus
                          ,
                        MiddleName = employeeDto.MiddleName,
                        PositionId = employeeDto.PositionId
                           ,
                        RoleId = employeeDto.RoleId
                           ,
                        UserName = employeeDto.UserName
                        , EmergencyContactPhone = employeeDto.EmergencyContactPhone
                        , EmergencyContactName = employeeDto.EmergencyContactName
                        

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
                        LogErrorHelper.LogError("First name is required.", null, LogErrorHelper.ErrorLevel.Warn);
                        return Results.BadRequest("First name is required.");
                    }

                    if (string.IsNullOrWhiteSpace(employeeDto.LastName))
                    {
                        LogErrorHelper.LogError("Last name is required.", null, LogErrorHelper.ErrorLevel.Warn);
                        return Results.BadRequest("Last name is required.");
                    }

                    if (string.IsNullOrWhiteSpace(employeeDto.Email))
                    {
                        LogErrorHelper.LogError("Email is required.", null, LogErrorHelper.ErrorLevel.Warn);
                        return Results.BadRequest("Email is required.");
                    }

                    // Basic email format validation
                    if (!employeeDto.Email.Contains("@"))
                    {
                        LogErrorHelper.LogError("Invalid email format.", null, LogErrorHelper.ErrorLevel.Warn);
                        return Results.BadRequest("Invalid email format.");
                    }

                    if (employeeDto.DepartmentId <= 0)
                    {LogErrorHelper.LogError("Valid department ID is required.", null, LogErrorHelper.ErrorLevel.Warn);
                        return Results.BadRequest("Valid department ID is required.");
                    }

                    // Optional: Validate hire date is not in the future
                    if (employeeDto.HireDate > DateTime.Now)
                    {LogErrorHelper.LogError("Hire date cannot be in the future.", null, LogErrorHelper.ErrorLevel.Warn);
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
                        Status = employeeDto.Status,
                        EmergencyContactName = employeeDto.EmergencyContactName,
                        EmergencyContactPhone = employeeDto.EmergencyContactPhone,
                        Gender = employeeDto.Gender
                           ,
                        MaritalStatus = employeeDto.MaritalStatus
                            ,
                        MiddleName = employeeDto.MiddleName
                            ,
                        Password = employeeDto.Password,
                        UserName = employeeDto.UserName,



                        TerminationDate = employeeDto.TerminationDate
                        
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

            group.MapDelete("/{id:int}", async (int id, IEmployeeService employeeService) =>
            {
                try
                {
                    var existingEmployee = await employeeService.GetEmployeeByIdAsync(id);
                    if (existingEmployee is null)
                    {
                        LogErrorHelper.LogError($"Employee with ID {id} not found for termination.", null, LogErrorHelper.ErrorLevel.Warn);
                        return Results.NotFound($"Employee with ID {id} not found.");
                    }
                    if (existingEmployee.Status == EmployeeStatus.Terminated)
                    {
                        LogErrorHelper.LogError($"Employee with ID {id} is already terminated.", null, LogErrorHelper.ErrorLevel.Warn);
                        return Results.BadRequest($"Employee with ID {id} is already terminated.");
                    }
                    var terminationDate = DateTime.Now;
                    var reason = "Terminated via API"; // You might want to pass this as a parameter
                    var success = await employeeService.TerminateEmployeeAsync(id, terminationDate, reason);
                    if (success)
                    {
                        LogErrorHelper.LogError($"Employee with ID {id} terminated successfully.", null, LogErrorHelper.ErrorLevel.Info);
                        return Results.Ok($"Employee with ID {id} terminated successfully.");
                    }
                    else
                    {
                        LogErrorHelper.LogError($"Failed to terminate employee with ID {id}.", null, LogErrorHelper.ErrorLevel.Error);
                        return Results.Problem("Failed to terminate the employee.");
                    }
                }
                catch (Exception ex)
                {
                    LogErrorHelper.LogError($"Error terminating employee with ID {id}", ex, LogErrorHelper.ErrorLevel.Error);
                    return Results.Problem("An error occurred while processing your request.");
                }
            })
                .WithName("TerminateEmployee")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status500InternalServerError);
        }
    }
}
