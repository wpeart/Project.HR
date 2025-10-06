using Project.HR.Domain.DTOs;
using Project.HR.Domain.Helpers;
using Project.HR.Domain.Interfaces;
using Project.HR.Domain.Models;

namespace Project.HR.WebAPI.Endpoints
{
    public static class DepartmentEndpoint
    {

        public static void MapDepartmentEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/departments").WithTags(nameof(Department));
            group.MapGet("/", async (IDepartmentService departmentService) =>
            {
                try
                {
                    var departments = await departmentService.GetAllDepartmentsAsync();
                    return Results.Ok(departments);
                }
                catch (Exception ex)
                {
                    LogErrorHelper.LogError("Error fetching departments", ex, LogErrorHelper.ErrorLevel.Error, "Get All Departments");
                    return Results.Problem("An error occurred while fetching departments.");
                }
            })
                .WithName("GetAllDepartments")
                .Produces<List<Department>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status500InternalServerError);

            group.MapGet("/{departmentName}", async (string departmentName, IDepartmentService departmentService) =>
            {
                try
                {
                    var department = await departmentService.GetDepartmentByNameAsync(departmentName);
                    return department is not null ? Results.Ok(department) : Results.NotFound();
                }
                catch (Exception ex)
                {
                    LogErrorHelper.LogError("Error fetching department by name", ex, LogErrorHelper.ErrorLevel.Error, "Get Department By Name");
                    return Results.Problem("An error occurred while fetching the department.");
                }
            })
                .WithName("GetDepartmentByName")
                .Produces<Department>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .Accepts<string>("application/json");

            group.MapGet("/{id:int}", async (int id, IDepartmentService departmentService) =>
            {
                try
                {
                    var department = await departmentService.GetDepartmentByIdAsync(id);
                    return department is not null ? Results.Ok(department) : Results.NotFound();
                }
                catch (Exception ex)
                {
                    LogErrorHelper.LogError("Error fetching department by id", ex, LogErrorHelper.ErrorLevel.Error, "Get Department By Id");
                    return Results.Problem("An error occurred while fetching the department.");
                }
            })
                .WithName("GetDepartmentById")
                .Produces<Department>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError);
                
            group.MapPost("/", async (DepartmentDTO departmentDto, IDepartmentService departmentService) =>
            {
                try
                {
                    if (string.IsNullOrEmpty(departmentDto.Name))
                    {
                        return Results.BadRequest("Department Name is required!");
                    }
                    Department department = new Department()
                    {
                        Name = departmentDto.Name,
                        Description = departmentDto.Description,
                        Code = departmentDto.Code,
                        Budget = departmentDto.Budget,
                        ParentDepartmentId = departmentDto.ParentDepartmentId
                    };
                    var createdDepartment = await departmentService.CreateDepartmentAsync(department);
                    return Results.Created($"/api/departments/{createdDepartment.Name}", createdDepartment);
                }
                catch (Exception ex)
                {
                    LogErrorHelper.LogError("Error fetching department by name", ex, LogErrorHelper.ErrorLevel.Error, "Get Department By Name");
                    return Results.Problem("An error occurred while fetching the department.");
                }
            })
                .Accepts<DepartmentDTO>("application/json")
                .WithName("CreateDepartment")
                .Produces<Department>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status500InternalServerError);

            group.MapPut("/{id:int}", async (int id, DepartmentDTO departmentDto, IDepartmentService departmentService) =>
            {
                try
                {
                    Department department = new Department()
                    {
                        Name = departmentDto.Name,
                        Description = departmentDto.Description,
                        Code = departmentDto.Code,
                        Budget = departmentDto.Budget,
                        ParentDepartmentId = departmentDto.ParentDepartmentId
                    };
                    var updatedDepartment = await departmentService.UpdateDepartmentAsync(id, department);
                    return updatedDepartment is not null ? Results.Ok(updatedDepartment) : Results.NotFound();
                }
                catch (Exception ex)
                {
                    LogErrorHelper.LogError("Error updating department", ex, LogErrorHelper.ErrorLevel.Error, "Put Update Department");
                    return Results.Problem("An error occurred while updating the department.");
                }
            })
                .Accepts<DepartmentDTO>("application/json")
                .WithName("UpdateDepartment")
                .Produces<Department>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError);

            group.MapDelete("/{id}", async (int id, IDepartmentService departmentService) =>
            {
                try
                {
                    

                    await departmentService.DeleteDepartmentAsync(id);
                    return Results.NoContent();

                }
                catch (Exception ex)
                {
                    LogErrorHelper.LogError("Error deleting department", ex, LogErrorHelper.ErrorLevel.Error, "Delete Update Department");
                    return Results.Problem("An error occurred while deleting the department.");
                }
            })
                .Accepts<string>("application/json")
                .WithName("DeleteDepartment")
                .Produces<bool>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status500InternalServerError);

        }
    }
}
