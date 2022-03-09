using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ATPL_Complaint.Models
{
    public class client
    {
        public static bool EnableSsl { get; internal set; }
        public static NetworkCredential Credentials { get; internal set; }
        public static SmtpDeliveryMethod DeliveryMethod { get; internal set; }
        public int id { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "COMPANY ID")]
        [DataType(DataType.Text)]
        public string company_id { get; set; }


        [Required(ErrorMessage ="*")]
        [Display(Name = "COMPANY NAME")]
        [DataType(DataType.Text)]
        public string company_name { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "CONTACT PERSON")]
        public string contact_person { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "ADDRESS")]
        public string address { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "CITY")]
        public string city { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "STATE")]
        public string state { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "PINCODE")]
        public string pincode { get; set; }

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
        [Display(Name = "PASSWORD")]
        public string pwd { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "GST.No.")]
        public string gst_no { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "PANCARD NUMBER")]
        public string pan_no { get; set; }

        public string cap { get; set; }
    }
}