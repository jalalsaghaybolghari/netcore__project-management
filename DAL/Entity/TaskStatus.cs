using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class TaskStatus
    {
        public long Id { get; set; }

        public long ProjectTaskId { get; set; }

        public ProjectTask ProjectTask { get; set; }

        public Status Status { get; set; }

        public DateTime Time { get; set; }
    }

    public enum Status
    {
        NotAction = 0,
        Pending =1,
        InProgress =2,
        Completed =3
    }
}
