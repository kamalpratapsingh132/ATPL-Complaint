using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATPL_Complaint.Models
{
    public class resetpasswordModel
    {
            [Display(Name = "EMAIL")]
            [Required(ErrorMessage = "EMAIL is Required.")]
            [DataType(DataType.EmailAddress)]
             public string email { get; set; }
            

            [Display(Name = "New Password")]
            [Required(ErrorMessage = "New Password is Required.")]
            public string newpassword { get; set; }

            [Display(Name = "Confirm New Password")]
            [Required(ErrorMessage = "Confirm New Password is Required.")]
            [Compare(otherProperty: "NewPassword", ErrorMessage = "New Password Does't Match")]
            [DataType(DataType.Password)]
            public string confirmnewpassword { get; set; }

            public int otp { get; set; }
    }
}