using DAL.Entity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class TaskFilter
    {
        public long? TaskId { get; set; }

        public long? ProjectId { get; set; }

        public int? Priority { get; set; }

        public DateTime? DeadLine { get; set; }

        public Status? TaskStatus { get; set; }

        public TaskSortType SortType { get; set; }
        public SortOrder SortOrder { get; set; }

    }

    public enum TaskSortType
    {
        Priority =1,
        Deadline =2,
        Id = 3,
        Title = 4,
        ProjectTitle = 5,
        Description = 6
    }
}
