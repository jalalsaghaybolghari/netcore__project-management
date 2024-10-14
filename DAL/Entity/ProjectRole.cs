using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class ProjectRole
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public List<ProjectMember>? ProjectMembers { get; set; }
        public List<RolePermission>? RolePermissions { get; set; }
    }
}
