using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class ProjectRolePermission
    {
        public long RoleId { get; set; }

        public string RoleTitle { get; set; }

        public long ProjectId { get; set; }

        public List<string>? PermitedActions { get; set; }

        public string ProjectTitle { get; set; }
    }
}
