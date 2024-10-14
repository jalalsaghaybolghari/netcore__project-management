using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class AddRoleWithPermissionDto
    {
        public string RoleTitle { get; set; }

        public List<string> PermitedActions { get; set; }        
    }
}
