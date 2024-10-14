using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(30)]
        public string? FirstName { get; set; }

        [MaxLength(30)]
        public string? LastName { get; set; }

        [MaxLength(11)]
        public string? Mobile { get; set; }

        public List<ProjectMember>? ProjectMembers { get; set; }
        public List<TaskMember>? TaskMembers { get; set; }
    }
}
