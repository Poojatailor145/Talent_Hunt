using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web;

namespace TalentHunt.ModelView
{
    public partial class productionv
    {
        public int pid { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Production Name")]
        [DataType(DataType.Text)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Invalid Production")]
        [RegularExpression(@"^[a-zA-z ]*$", ErrorMessage = "Invalid Production")]
        public string pname { get; set; }

        [DisplayName("Image")]
        public string pimage { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Head Name")]
        [DataType(DataType.Text)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Invalid Name")]
        [RegularExpression(@"^[a-zA-z ]*$", ErrorMessage = "Invalid Name")]
        public string phead { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Address")]
        public string address { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Contact Number")]
        [RegularExpression(@"^[0-9]{10}$",ErrorMessage = "Invalid Number")]
        public long contactno { get; set; }

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

        [Required(ErrorMessage = "*")]
        [DisplayName("Description")]
        [DataType(DataType.Text)]
        public string description { get; set; }
        public string status { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        public virtual ICollection<eventrequirev> eventrequires { get; set; }
        public virtual ICollection<productioneventv> productionevents { get; set; }
        public virtual ICollection<subproductionv> subproductions { get; set; }
        public virtual ICollection<subuserv> subusers { get; set; }
        public virtual ICollection<userapplyv> userapplies { get; set; }
    }
}