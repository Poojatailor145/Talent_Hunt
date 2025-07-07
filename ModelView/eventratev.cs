using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TalentHunt.ModelView
{
    public partial class eventratev
    {
        public int erid { get; set; }
        public int peid { get; set; }
        public int userid { get; set; }
        [DisplayName("Rating")]
        [Range(0, 5, ErrorMessage = "Invalid Rating")]
        public float rating { get; set; }

        [DisplayName("Comment")]
        public string comment { get; set; }
    }
}