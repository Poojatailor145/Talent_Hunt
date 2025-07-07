using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TalentHunt.ModelView
{
    public partial class subuserv
    {
        public int suid { get; set; }
        public int planid { get; set; }
        public int userid { get; set; }
        
        [Required(ErrorMessage = "*")]
        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTime startdate { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        public DateTime enddate { get; set; }
    }
}