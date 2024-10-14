using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class TaskMember
    {
        public long Id { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public bool IsDelete { get; set; }

        public int? Responsibility { get; set; }

        public long ProjectTaskId { get; set; }

        public ProjectTask ProjectTask { get; set; }

    }
}
