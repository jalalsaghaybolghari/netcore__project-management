using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }        

        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }
    }
}
