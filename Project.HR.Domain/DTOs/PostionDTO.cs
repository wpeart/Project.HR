
using Project.HR.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.HR.Domain.DTOs
{
    public class PostionDTO
    {
        public int? Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Code { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal MinSalary { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal MaxSalary { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;

        public EmploymentType EmploymentType { get; set; }
    }
}
