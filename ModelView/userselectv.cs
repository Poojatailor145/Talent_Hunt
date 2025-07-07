using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TalentHunt.ModelView
{
    public partial class userselectv
    {
        public int usid { get; set; }
        public int peid { get; set; }
        public int userid { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Final Pay")]
        [RegularExpression(@"^[0-9]$",ErrorMessage = "Invalid Value")]
        public int finalpay { get; set; }

        public virtual productioneventv productionevent { get; set; }
        public virtual userv user { get; set; }
    }
}