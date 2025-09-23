using Project.HR.Domain.Enums;
using Project.HR.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.HR.Domain.Models
{
    public class Leave
    {
        public int Id { get; set; }

        public LeaveType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DaysRequested { get; set; }

        [MaxLength(1000)]
        public string Reason { get; set; } = string.Empty;

        public LeaveStatus Status { get; set; }

        [MaxLength(500)]
        public string? ApproverComments { get; set; }

        public DateTime RequestDate { get; set; }
        public DateTime? ApprovalDate { get; set; }

        public int EmployeeId { get; set; }
        public int? ApproverId { get; set; }

        // Navigation Properties
        public Employee Employee { get; set; } = null!;
        public Employee? Approver { get; set; }
    }
}
