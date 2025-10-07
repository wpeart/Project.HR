using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Project.HR.Data.DAL;
using Project.HR.Data.Models;
using Project.HR.Domain.Interfaces;
using Project.HR.WebAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<HRDbContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEmployeeService, EmployeeDAL>();
builder.Services.AddScoped<IRoleService, RolesDAL>();
builder.Services.AddScoped<IDepartmentService, DepartmentDAL>();
builder.Services.AddScoped<IPositionService, PositionDAL>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HR Back Office API",
        Version = "v1",
        Description = "A comprehensive HR management system API",
        Contact = new OpenApiContact
        {
            Name = "HR Team",
            Email = "hr@company.com"
        }
    });
});

var app = builder.Build();

using (var scope=app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HRDbContext>();
    db.Database.EnsureCreated();    
}

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{*/
    app.UseSwagger();
    app.MapOpenApi();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HR API v1");
        c.RoutePrefix = string.Empty; // Makes Swagger UI available at root (/)
        // Or use: c.RoutePrefix = "swagger"; // Available at /swagger
    });
//}

app.UseHttpsRedirection();
//app.UseAuthorization();

app.MapRoleEndpoints();
app.MapPositionEndpoints();
app.MapDepartmentEndpoints();
app.MapEmployeeEndpoints();


app.Run();


