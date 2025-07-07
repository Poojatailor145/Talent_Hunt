using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web;

namespace TalentHunt.ModelView
{
    public partial class adminv
    {
        public int aid { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Admin Name")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Invalid Name")]
        [RegularExpression(@"^[a-zA-z ]*$", ErrorMessage = "Invalid Name")]
        public string aname { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Age")]
        [Range(1, 90, ErrorMessage = "Invalid Age")]
        [RegularExpression(@"^[0-9]{0,3}$", ErrorMessage = "Invalid age")]
        public int age { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Gender")]
        public string gender { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Email")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Invalid Email")]
        [RegularExpression(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$", ErrorMessage = "Invalid Name")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("User Name")]
        [RegularExpression(@"^[a-zA-z@_.0-9 ]*$", ErrorMessage = "Invalid Username")]
        public string username { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [DisplayName("Save credentials")]
        public bool RememberMe { get; set; }
        public string photo { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }
    }
}