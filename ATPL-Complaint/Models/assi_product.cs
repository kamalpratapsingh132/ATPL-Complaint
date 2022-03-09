using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATPL_Complaint.Models
{
    public class assi_product
    {
        public int id { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "SELECT CLIENT ID")]
        public string select_client_id { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "SELECT PRODUCT")]
        public string select_product { get; set; }

        public List<product> product_name { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "SELECT CLIENT")]
        public string select_client { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "SELECT EMPLOYEE")]
        public string select_emp { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "SELECT PROJECT SUPPORT TEAM")]
        public string select_project_supp { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "INSTALLATION DATE")]
        public string installation_date { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "AMC DATE")]
        public string amc_date { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "AMOUNT")]
        public string amount { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "AMC %")]
        public string amc_per { get; set; }
    }
}