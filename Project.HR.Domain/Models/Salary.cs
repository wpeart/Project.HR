using Project.HR.Domain.Enums;
using Project.HR.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.HR.Domain.Models
{
    public class Salary
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        public SalaryType Type { get; set; } // Hourly, Monthly, Annual
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }

        [MaxLength(500)]
        public string? Reason { get; set; }

        public int EmployeeId { get; set; }

        // Navigation Properties
        public Employee Employee { get; set; } = null!;
    }
}
