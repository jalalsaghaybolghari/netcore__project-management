using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class ProjectTask
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public int Priority { get; set; }

        public DateTime? DeadLine { get; set; }

        public long? ProjectId { get; set; }

        public Project? Project { get; set; }

        public List<TaskMember>? TaskMembers { get; set; }

        public List<TaskStatus>? TaskStatuses { get; set; }


    }
}
