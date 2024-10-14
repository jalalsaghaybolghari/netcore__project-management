using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class ProjectMembersResult
    {
        public long Id { get; set; }
        public string UserName { get; set; }

        public string ProjectRole { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }

    }
}
