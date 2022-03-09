using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATPL_Complaint.Models
{
    public class sadmin
    {
        public int id { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name= "NAME")]
        public string name { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "EMAIL ID")]
        public string email { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "PASSWORD")]
        public string pwd { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "CONTACT NUMBER")]
        public string contactno { get; set; }

        [Required(ErrorMessage = "*")]
        public string cap { get; set; }
    }
}