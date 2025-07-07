using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TalentHunt.ModelView
{
    public partial class ratingv
    {
        public int rid { get; set; }
        public int userid { get; set; }
        public int peid { get; set; }

        [DisplayName("Rating")]
        [Range(0,5,ErrorMessage = "Invalid Rating")]
        public float rating1 { get; set; }

        [DisplayName("Comment")]
        public string comment { get; set; }
    }
}