using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class UpdateProfileModel
    {        
        public string? FirstName { get; set; }
        
        public string? LastName { get; set; }

        public string? Mobile { get; set; }
    }
}
