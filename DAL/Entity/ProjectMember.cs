using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class ProjectMember
    {
        public long Id { get; set; }

        public long ProjectId { get; set; }

        public Project Project { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public long ProjectRoleId { get; set; }

        public ProjectRole ProjectRole { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public bool IsDelete { get; set; }
    }
}
