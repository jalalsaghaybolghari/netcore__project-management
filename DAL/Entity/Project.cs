using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class Project
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public List<ProjectMember>? ProjectMembers { get; set; }
        public List<ProjectTask>? ProjectTasks { get; set; }
    }
}
