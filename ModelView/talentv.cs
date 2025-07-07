using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TalentHunt.ModelView
{
    public partial class talentv
    {
        public int tid { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Talents")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Invalid Talent")]
        [RegularExpression(@"^[a-zA-z ]*$", ErrorMessage = "Invalid Talent")]
        public string ttype { get; set; }

        public virtual ICollection<eventrequirev> eventrequires { get; set; }
        public virtual ICollection<imagev> images { get; set; }
        public virtual ICollection<userprofilev> userprofiles { get; set; }
        public virtual ICollection<videov> videos { get; set; }
    }
}