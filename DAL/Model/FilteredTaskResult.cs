using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class FilteredTaskResult
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public int Priority { get; set; }

        public DateTime? DeadLine { get; set; }

        public long? ProjectId { get; set; }
        public string? ProjectTitle { get; set; }


        public Status? TaskStatus { get; set; }

    }
}
