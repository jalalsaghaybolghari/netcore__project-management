using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class RolePermission
    {
        public long Id { get; set; }

        public string Action { get; set; }

        public long ProjectRoleId { get; set; }

        public ProjectRole ProjectRole { get; set; }
    }
}
