using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web;

namespace TalentHunt.ModelView
{
    public class imagev
    {
        public int iid { get; set; }
        public int userid { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Talent")]
        public int tid { get; set; }

        [DisplayName("Image")]
        public string image1 { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Description")]
        public string caption { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }
        public virtual talentv talent { get; set; }
        public virtual userv user { get; set; }
    }
}