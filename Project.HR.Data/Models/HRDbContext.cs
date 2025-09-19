using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.HR.Data.Models
{
    public class HRDbContext : DbContext
    {
        DbSet<Employee> Employees { get; set; }
        DbSet<Department> Departments { get; set; }
        DbSet<Position> Positions { get; set; }
        DbSet<Salary> Salaries { get; set; }
        DbSet<Leave> Leaves { get; set; }
        DbSet<UserRoles> UserRoles { get; set; }


        public HRDbContext(DbContextOptions<HRDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.HasIndex(e => e.UserName).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasOne(e => e.Manager)
                      .WithMany(e => e.DirectReports)
                      .HasForeignKey(e => e.ManagerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Department)
                .WithMany(d => Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Position)
                       .WithMany(p => p.Employees)
                       .HasForeignKey(e => e.PositionId)
                       .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.HasIndex(d => d.Code).IsUnique();

                // Self-referencing relationship for parent department
                entity.HasOne(d => d.ParentDepartment)
                      .WithMany(d => d.SubDepartments)
                      .HasForeignKey(d => d.ParentDepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasIndex(p => p.Code).IsUnique();

                entity.HasOne(p => p.Department)
                      .WithMany(d => d.Positions)
                      .HasForeignKey(p => p.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Salary>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.HasOne(s => s.Employee)
                      .WithMany(e => e.Salaries)
                      .HasForeignKey(s => s.EmployeeId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Index for efficient salary history queries
                entity.HasIndex(s => new { s.EmployeeId, s.EffectiveDate });
            });

            modelBuilder.Entity<Leave>(entity =>
            {
                entity.HasKey(l => l.Id);

                entity.HasOne(l => l.Employee)
                      .WithMany(e => e.Leaves)
                      .HasForeignKey(l => l.EmployeeId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(l => l.Approver)
                      .WithMany()
                      .HasForeignKey(l => l.ApproverId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Index for leave queries
                entity.HasIndex(l => new { l.EmployeeId, l.StartDate, l.EndDate });
            });

        }
    }


}
