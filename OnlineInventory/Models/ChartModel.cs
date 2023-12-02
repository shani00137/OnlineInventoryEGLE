using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineInventory.Models
{
    public class ChartModel
    {
        public String InvoiceDate { set; get; }
        public Nullable<double> Amount { get; set; }
    }
}