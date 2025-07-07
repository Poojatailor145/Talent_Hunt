using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TalentHunt.ModelView
{
    public partial class checkpass
    {
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "*")]
        public string password { get; set; }

        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "*")]
        [Compare("password", ErrorMessage = "Do not match")]
        public string cpassword { get; set; }
    }
}