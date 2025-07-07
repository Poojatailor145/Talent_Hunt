using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TalentHunt.ModelView
{
    public partial class reset
    {
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "*")]
        public string email { get; set; }

        [DisplayName("OTP")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "*")]
        public string otp { get; set; }

        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "*")]
        public string password { get; set; }

        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Password not same")]
        public string confirmpassword { get; set; }
    }
}