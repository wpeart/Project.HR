using Project.HR.Domain.DTOs;
using Project.HR.Domain.Helpers;
using Project.HR.Domain.Interfaces;
using Project.HR.Domain.Models;
using System.Data;

namespace Project.HR.WebAPI.Endpoints
{
    public static class PositionEndpoint
    {

        public static void MapPositionEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/positions").WithTags("Positions");

            group.MapGet("/", async (IPositionService positionService) =>
            {
                try
                {
                    var positions = await positionService.GetAllPositionsAsync();
                    return Results.Ok(positions);
                }
                catch (Exception ex)
                {

                    LogErrorHelper.LogError("Error creating role", ex, LogErrorHelper.ErrorLevel.Error, "Post Creating Role");
                    return Results.Problem("An error occurred while creating the role.");
                }

            })
                .WithName("GetAllPositions")
                .Produces<List<Position>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status500InternalServerError);
            group.MapGet("/{positionName}", async (string positionName, IPositionService positionService) =>
            {
                try
                {
                    return await positionService.GetPositionByNameAsync(positionName) is Position position
                       ? Results.Ok(position)
                       : Results.NotFound();
                }
                catch (Exception ex)
                {

                    LogErrorHelper.LogError("Error creating role", ex, LogErrorHelper.ErrorLevel.Error, "Post Creating Role");
                    return Results.Problem("An error occurred while creating the role.");
                }
            })
                .WithName("GetPositionByName")
                .Produces<Position>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError);

            group.MapPost("/", async (PostionDTO positionDto, IPositionService positionService) =>
            {
                try
                {
                    // Validation - Fixed error message
                    if (string.IsNullOrWhiteSpace(positionDto.Title))
                    {
                        return Results.BadRequest("Position title is required.");
                    }

                    // Additional validations you might want
                    if (string.IsNullOrWhiteSpace(positionDto.Code))
                    {
                        return Results.BadRequest("Position code is required.");
                    }

                    if (positionDto.DepartmentId <= 0)
                    {
                        return Results.BadRequest("Valid department ID is required.");
                    }

                    if (positionDto.MinSalary < 0 || positionDto.MaxSalary < 0)
                    {
                        return Results.BadRequest("Salary values must be non-negative.");
                    }

                    if (positionDto.MinSalary > positionDto.MaxSalary)
                    {
                        return Results.BadRequest("Minimum salary cannot be greater than maximum salary.");
                    }

                    // Fixed logging message
                    LogErrorHelper.LogError($"Creating position: {positionDto.Title}", null, LogErrorHelper.ErrorLevel.Info);

                    Position position = new Position()
                    {
                        Title = positionDto.Title.Trim(),
                        Description = positionDto.Description?.Trim(),
                        Code = positionDto.Code.Trim(),
                        DepartmentId = positionDto.DepartmentId,
                        MaxSalary = positionDto.MaxSalary,
                        MinSalary = positionDto.MinSalary
                    };

                    var createdPosition = await positionService.CreatePositionAsync(position);

                    // Fixed: Remove 'await' before Results.Created (Results.Created is not async)
                    return Results.Created($"/api/positions/{createdPosition.Id}", createdPosition);
                }
                catch (Exception ex)
                {
                    // Fixed: Corrected log message and caller name
                    LogErrorHelper.LogError("Error creating position", ex);
                    return Results.Problem("An error occurred while creating the position.");
                }
            })
            .WithName("CreatePosition")
            .Accepts<PostionDTO>("application/json")  // Added: Specify what it accepts
            .Produces<Position>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)  // Added: For validation errors
            .ProducesProblem(StatusCodes.Status500InternalServerError);

            group.MapPut("/{positionName}", async (string positionName, PostionDTO positionDto, IPositionService positionService) =>
            {
                try
                {
                    Position position = new Position()
                    {
                        Title = positionDto.Title.Trim(),
                        Description = positionDto.Description?.Trim(),
                        Code = positionDto.Code.Trim(),
                        DepartmentId = positionDto.DepartmentId,
                        MaxSalary = positionDto.MaxSalary,
                        MinSalary = positionDto.MinSalary
                    };
                    var updatedPosition = await positionService.UpdatePositionAsync(positionName, position);
                    return updatedPosition is not null ? Results.Ok(updatedPosition) : Results.NotFound();
                }
                catch (Exception ex)
                {
                    LogErrorHelper.LogError("Error creating role", ex, LogErrorHelper.ErrorLevel.Error, "Post Creating Role");
                    return Results.Problem("An error occurred while creating the role.");
                }
            }).WithName("UpdatePosition")
            .Accepts<PostionDTO>("application/json")
            .Produces<Position>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

            group.MapDelete("/{positionName}", async (string positionName, IPositionService positionService) =>
            {
                try
                {
                    var deleted = await positionService.DeletePositionAsync(positionName);
                    return deleted ? Results.NoContent() : Results.NotFound();
                }
                catch (Exception ex)
                {
                    LogErrorHelper.LogError("Error creating role", ex, LogErrorHelper.ErrorLevel.Error, "Post Creating Role");
                    return Results.Problem("An error occurred while creating the role.");
                }
            })
                .WithName("DeletePosition")
                .Accepts<string>("application/json")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status500InternalServerError);

        }
    }
}
