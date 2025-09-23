using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.HR.Domain.Models
{
    public class UserRoles
    {
        public int RoleId { get; set; }
        [Required, MaxLength(30)]
        public string RoleName { get; set; }
        [MaxLength(100)]
        public string RoleDescription { get; set; }

        public List<Employee> Employees { get; set; }

    }
}
