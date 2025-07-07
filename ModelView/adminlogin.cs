using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TalentHunt.ModelView
{
    public partial class adminlogin
    {
        [Required(ErrorMessage = "*")]
        [DisplayName("User Name")]
        [RegularExpression(@"^[a-zA-z@_.0-9 ]*$", ErrorMessage = "Invalid Username")]
        public string username { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        public bool rememberme { get; set; }
    }
}