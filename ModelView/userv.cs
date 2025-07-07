using System.Collections.Generic;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace TalentHunt.ModelView
{
    public partial class userv
    {

        public int userid { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("First Name")]
        [DataType(DataType.Text)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Invalid First Name")]
        [RegularExpression(@"^[a-zA-z ]*$", ErrorMessage = "Invalid First Name")]
        public string fname { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Last Name")]
        [DataType(DataType.Text)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Invalid Last Name")]
        [RegularExpression(@"^[a-zA-z ]*$", ErrorMessage = "Invalid Last Name")]
        public string lname { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Gender")]
        public string gender { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Age")]
        [Range(13,90,ErrorMessage = "Invalid age")]
        [RegularExpression(@"^[0-9]{0,3}$", ErrorMessage = "Invalid age")]
        public int age { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Address")]
        [DataType(DataType.MultilineText)]
        public string address { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("City")]
        [DataType(DataType.Text)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Invalid City")]
        [RegularExpression(@"^[a-zA-z ]*$", ErrorMessage = "Invalid City")]
        public string city { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("State")]
        [DataType(DataType.Text)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Invalid State")]
        [RegularExpression(@"^[a-zA-z ]*$", ErrorMessage = "Invalid State")]
        public string state { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Pincode")]
        [RegularExpression(@"^[0-9]{6}$",ErrorMessage = "Invalid Pincode")]
        public int pincode { get; set; }

        [DisplayName("Photo")]
        public string photo { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Email")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Invalid Email")]
        [RegularExpression(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$", ErrorMessage = "Invalid Name")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("User Name")]
        [DataType(DataType.Text)]
        public string username { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        public string status { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

    }
}