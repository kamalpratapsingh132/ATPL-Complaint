using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ATPL_Complaint.Models
{
    public class product
    {
        public int id { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "PRODUCT ID")]
        public string product_id { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "PRODUCT CATEGORY")]
        public string product_category { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "PRODUCT NAME")]
        public string product_name { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "DESCRIPTION")]
        public string description { get; set; }

        [NotMapped]
        public List<caty> ProductCollection { get; set; }

    }
}