using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATPL_Complaint.Models
{
    public class support_ticket
    {
        public int id { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name ="SELECT PRODUCT")]
        public string select_product { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "TICKET FOR")]
        public string ticket_for { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "DATE")]
        public string date { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "MESSAGE")]
        public string message { get; set; }
        public string status { get; set; }
    }
}