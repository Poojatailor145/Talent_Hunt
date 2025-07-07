using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace TalentHunt.ModelView
{
    public class videov
    {
        public int vid { get; set; }
        public int userid { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Talent")]
        public int tid { get; set; }

        [DisplayName("Video")]
        public string video1 { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Caption")]
        [DataType(DataType.Text)]
        public string caption { get; set; }

        public HttpPostedFileBase VideoFile { get; set; }

        public virtual talentv talent { get; set; }
        public virtual userv user { get; set; }
    }
}