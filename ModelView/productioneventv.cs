using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using System.Web;
using System.Collections.Generic;

namespace TalentHunt.ModelView
{
    public partial class productioneventv
    {
        public int peid { get; set; }
        public int pid { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Event Name")]
        public string ename { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Event Type")]
        public string etype { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Event Manager")]
        public string emanager { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime startdate { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime enddate { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Event Venue")]
        public string evenu { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Event Visitors")]
        public int evisitors { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("DeadLine")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime appdeadline { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string description { get; set; }
        public string status { get; set; }

        [DisplayName("Image")]
        public string image { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        public virtual ICollection<eventrequirev> eventrequires { get; set; }
        public virtual productionv production { get; set; }
        public virtual ICollection<ratingv> ratings { get; set; }        
        public virtual ICollection<userapplyv> userapplies { get; set; }
        public virtual ICollection<userselectv> userselects { get; set; }
    }
}