using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Project.HR.Data.Models
{
    public class Employee
    {
        public int UserId { get; set; }
        [Required, MaxLength(30)]
        public string UserName { get; set; }

        [Required, MaxLength(50)]
        public string Password { get; set; }
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string MiddleName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public Gender Gender { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public EmployeeStatus Status { get; set; }

        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(50)]
        public string City { get; set; } = string.Empty;

        [MaxLength(50)]
        public string State { get; set; } = string.Empty;

        [MaxLength(10)]
        public string ZipCode { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Country { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? EmergencyContactName { get; set; }

        [MaxLength(20)]
        public string? EmergencyContactPhone { get; set; }

        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
        public int? ManagerId { get; set; }
        public int PositionId { get; set; }


        public Position Position { get; set; } = null!;
        public Employee? Manager { get; set; }
        public List<Employee> DirectReports { get; set; } = new();
        public List<Salary> Salaries { get; set; } = new();
        public List<Leave> Leaves { get; set; } = new();
        public Department Department { get; set; } = null!;



       

    }
}
