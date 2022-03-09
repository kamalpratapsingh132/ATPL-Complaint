using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ATPL_Complaint.Models
{
    public class caty
    {
      
        public int id { get; set; }
        public string caty_id { get; set; }
        public string product_caty { get; set; }

        
    }
}