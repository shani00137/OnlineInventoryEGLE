using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineInventory.Models
{
    public class DashaboardModel
    {
        public double TotalSale { get; set; }
        public double TodaySale { get; set; }
        public int Customers { get; set; }
        public double TotalDebtor { get; set; }
    }
}