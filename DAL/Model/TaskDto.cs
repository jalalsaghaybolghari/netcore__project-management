using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class TaskDto
    {
        public string Title { get; set; }

        public string? Description { get; set; }

        public long ProjectId { get; set; }

        public int Priority { get; set; }

        public DateTime? DeadLine { get; set; }
    }
}
