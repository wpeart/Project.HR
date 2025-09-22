using Project.HR.Domain.DTOs;
using Project.HR.Domain.Helpers;
using Project.HR.Domain.Interfaces;
using Project.HR.Domain.Models;

namespace Project.HR.WebAPI.Endpoints
{
    public static class RoleEndpoint
    {
        public static void MapRoleEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/roles").WithTags(nameof(UserRoles));
            group.MapGet("/", async (IRoleService roleService) =>
            {
                try
                {
                    var roles = await roleService.GetAllRolesAsync();
                    return Results.Ok(roles);
                }
                catch (Exception ex)
                {

                    LogErrorHelper.LogError("Error creating role", ex, LogErrorHelper.ErrorLevel.Error, "Post Creating Role");
                    return Results.Problem("An error occurred while creating the role.");
                }


            })
            .WithName("GetAllRoles")
            .Produces<List<UserRoles>>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status500InternalServerError);
            group.MapGet("/{roleId:int}", async (int roleId, IRoleService roleService) =>
            {
                try
                {
                    var role = await roleService.GetRoleByIdAsync(roleId);
                    return role is not null ? Results.Ok(role) : Results.NotFound();
                }
                catch (Exception ex)
                {

                    LogErrorHelper.LogError("Error creating role", ex, LogErrorHelper.ErrorLevel.Error, "Post Creating Role");
                    return Results.Problem("An error occurred while creating the role.");
                }
               
            })
            .WithName("GetRoleById")
            .Produces<UserRoles>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
            group.MapPost("/", async (UserRolesDTO role, IRoleService roleService) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(role.RoleName))
                    {
                        return Results.BadRequest("Role name is required.");
                    }

                    UserRoles dbRole = new UserRoles()
                    {
                        RoleDescription = role.RoleDescription,
                        RoleName = role.RoleName
                    };
                    var createdRole = await roleService.CreateRoleAsync(dbRole);
                    return Results.Created($"/api/roles/{createdRole.RoleId}", createdRole);
                }
                catch (Exception ex)
                {

                    LogErrorHelper.LogError("Error creating role", ex, LogErrorHelper.ErrorLevel.Error, "Post Creating Role");
                    return Results.Problem("An error occurred while creating the role.");
                }

            })
            .WithName("CreateRole")
            .Accepts<UserRoles>("application/json")
            .Produces<UserRoles>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError);



            group.MapPut("/{roleId:int}", async (int roleId, UserRolesDTO roleDto, IRoleService roleService) =>
            {
                try
                {
                    UserRoles dbRole = new UserRoles()
                    {
                        RoleId = roleId, // Important: set the ID for update
                        RoleDescription = roleDto.RoleDescription,
                        RoleName = roleDto.RoleName.Trim()
                    };

                    var updatedRole = await roleService.UpdateRoleAsync(roleId, dbRole);
                    return updatedRole is not null ? Results.Ok(updatedRole) : Results.NotFound();
                }
                catch (Exception ex)
                {

                    LogErrorHelper.LogError("Error creating role", ex, LogErrorHelper.ErrorLevel.Error, "Post Creating Role");
                    return Results.Problem("An error occurred while creating the role.");
                }
               
            })
            .WithName("UpdateRole")
            .Accepts<UserRoles>("application/json")
            .Produces<UserRoles>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
             .ProducesProblem(StatusCodes.Status500InternalServerError);
            group.MapDelete("/{roleId:int}", async (int roleId, IRoleService roleService) =>
            {
                try
                {
                    var deleted = await roleService.DeleteRoleAsync(roleId);
                    return deleted ? Results.NoContent() : Results.NotFound();
                }
                catch (Exception ex)
                {

                    LogErrorHelper.LogError("Error creating role", ex, LogErrorHelper.ErrorLevel.Error, "Post Creating Role");
                    return Results.Problem("An error occurred while creating the role.");
                }

            })
            .WithName("DeleteRole")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
             .ProducesProblem(StatusCodes.Status500InternalServerError);
        }
    }
}
