using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TalentHunt.ModelView
{
    public partial class subproductionv
    {
        public int spid { get; set; }
        public int planid { get; set; }
        public int pid { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTime startdate { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        public DateTime enddate { get; set; }


        public virtual planv plan { get; set; }
        public virtual productionv production { get; set; }
    }
}