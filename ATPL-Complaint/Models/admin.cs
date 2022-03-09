using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATPL_Complaint.Models
{
    public class admin
    {
        public int id { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "EMPLOYEE ID")]
        public string emp_id { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "EMPLOYEE NAME")]
        public string emp_name { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "CONTACT NUMBER")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                    ErrorMessage = "ENTERED NUMBER FORMET IS NOT VALID.")]
        public string contact_no { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "EMAIL ID")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email_id { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "ADDRESS")]
        public string address { get; set; }


        [Required(ErrorMessage = "*")]
        [Display(Name = "STATE")]
        public string state { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "CITY")]
        public string city { get; set; }


        [Required(ErrorMessage = "*")]
        [Display(Name = "PINCODE")]
        public string pincode { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "DESIGINATION")]
        public string desigination { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "DEPARTMENT")]
        public string department { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "PASSWORD")]
        public string pwd { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "CONTACT NUMBER")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                    ErrorMessage = "INVALID NUMBER")]
        public string contactno1 { get; set; }

        [Required(ErrorMessage = "*")]
        public string cap { get; set; }
}
}