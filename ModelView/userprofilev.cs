using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TalentHunt.ModelView
{
    public partial class userprofilev
    {
        public int upid { get; set; }
        public int userid { get; set; }
        public int tid { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Experience")]
        [DataType(DataType.Text)]
        [Range(0,90,ErrorMessage = "Invalid")]
        [RegularExpression(@"^[0-9]$",ErrorMessage = "Invalid")]
        public int experience { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Portfolio Link")]
        public string portfolio { get; set; }

        public virtual talentv talent { get; set; }
        public virtual userv user { get; set; }
    }
}