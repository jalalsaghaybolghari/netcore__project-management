using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class TaskStatusResult
    {
        public Status Status { get; set; }

        public DateTime Time { get; set; }
    }
}
