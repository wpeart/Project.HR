using Project.HR.Domain.DTOs;
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

                var roles = await roleService.GetAllRolesAsync();
                return Results.Ok(roles);
            })
            .WithName("GetAllRoles")
            .Produces<List<UserRoles>>(StatusCodes.Status200OK);
            group.MapGet("/{roleId:int}", async (int roleId, IRoleService roleService) =>
            {
                var role = await roleService.GetRoleByIdAsync(roleId);
                return role is not null ? Results.Ok(role) : Results.NotFound();
            })
            .WithName("GetRoleById")
            .Produces<UserRoles>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
            group.MapPost("/", async (UserRolesDTO role, IRoleService roleService) =>
            {
                UserRoles dbRole = new UserRoles() { 
                   RoleDescription=role.RoleDescription,
                     RoleName=role.RoleName
                };
                var createdRole = await roleService.CreateRoleAsync(dbRole);
                return Results.Created($"/api/roles/{createdRole.RoleId}", createdRole);
            })
            .WithName("CreateRole")
            .Accepts<UserRoles>("application/json")
            .Produces<UserRoles>(StatusCodes.Status201Created);
            group.MapPut("/{roleId:int}", async (int roleId, UserRoles role, IRoleService roleService) =>
            {
                var updatedRole = await roleService.UpdateRoleAsync(roleId, role);
                return updatedRole is not null ? Results.Ok(updatedRole) : Results.NotFound();
            })
            .WithName("UpdateRole")
            .Accepts<UserRoles>("application/json")
            .Produces<UserRoles>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
            group.MapDelete("/{roleId:int}", async (int roleId, IRoleService roleService) =>
            {
                var deleted = await roleService.DeleteRoleAsync(roleId);
                return deleted ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteRole")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
