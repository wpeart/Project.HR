using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.HR.Domain.DTOs
{
    public class UserRolesDTO
    {
       public int RoleId { get; set; }
        [Required, MaxLength(30)]
        public string RoleName { get; set; }
        [MaxLength(100)]
        public string? RoleDescription { get; set; }
    }
}
