using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TalentHunt.ModelView
{
    public partial class eventrequirev
    {
        public int erid { get; set; }
        public int pid { get; set; }
        public int peid { get; set; }
        public int tid { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Age Range")]
        [DataType(DataType.Text)]
        public string agerange { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Require Gender")]
        public string gender { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Expected Payment")]
        public string payrange { get; set; }

        public virtual productionv production { get; set; }
        public virtual productioneventv productionevent { get; set; }
        public virtual talentv talent { get; set; }
    }
}