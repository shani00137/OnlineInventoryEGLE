using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineInventory.Models
{
    public class AccountsMD
    {
    }
    public partial class ReceiptVoucherMD 
    {
        public int VoucherID { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherDate { get; set; }
        public string CustName { get; set; }
        public int CustomerID { get; set; }
        public double AmountReceived { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public String UserName { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class JournalEntriesMD
    {
        public int JEId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Source { get; set; }
        public string Reference { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public string Remarks { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
    }
}