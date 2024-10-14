using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class ChangeTaskStatusDto
    {
        public long TaskId { get; set; }

        public Status TaskStatus { get; set; }
    }
}
