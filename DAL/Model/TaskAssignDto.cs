using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class TaskAssignDto
    {
        public long TaskId { get; set; }
        public string UserName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? Responsibility { get; set; }
    }
}
