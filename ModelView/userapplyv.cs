using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TalentHunt.ModelView
{
    public partial class userapplyv
    {
        public int uaid { get; set; }
        public int pid { get; set; }
        public int peid { get; set; }
        public int userid { get; set; }

        [DataType(DataType.Date)]
        public DateTime appdate { get; set; }
        public string status { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Expected Payment")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Invalid Price")]
        public int expay { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Message")]
        [DataType(DataType.MultilineText)]
        public string message { get; set; }
    }
}