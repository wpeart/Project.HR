using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Project.HR.Data.Models
{
    public class Department
    {

        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Code { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public int? ParentDepartmentId { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal Budget { get; set; }

        // Navigation Properties
        public Department? ParentDepartment { get; set; }
        public List<Department> SubDepartments { get; set; } = new();
        public List<Employee> Employees { get; set; } = new();
        public List<Position> Positions { get; set; } = new();
    }
}
