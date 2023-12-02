using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineInventory.Models
{
    public class ReportMD
    {
        public int CustomerID { set; get; }
        public DateTime FromDate { set; get; }
        public DateTime ToDate { set; get; }
      
    }
}