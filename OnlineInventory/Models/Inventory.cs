using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineInventory.Models
{
    public class Inventory
    {
    }

    public class ItemsMD
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public double SaleRate { get; set; }
        public int? CustomerId { get; set; }
        public string ItemCode { get; set; }


    }

    public class CustomerMD
    {
        public Nullable<int> CustomerId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public double OpeningBal { get; set; }
    }

    public class CustomerItemsMD
    {
        public int CustomerItemId { get; set; }
        public int ItemId { get; set; }
        public int CustomerId { get; set; }
        public double SaleRate { get; set; }
    }

    public class InvoiceInfoMD: PageModel
    {
        public String ReturnInvoiceNo { get; set; }
        public String InvoiceNo { get; set; }

        public System.DateTime InvoiceDate { get; set; }
        public int CustomerId { get; set; }
        public string Remarks { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int ItemId { get; set; }
        public double Quantity { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }
        public double TotalAmount { get; set; }
        public string ItemName { get; set; }
        public List<InvoiceInfoMD> InvoiceList { get; set; }
        public  List<CustomerMD> CustomerList { get; set; }
    }

    public class InvoiceDetailMD
    {
        public int RecordId { get; set; }
        public int ItemId { get; set; }
        public int InvoiceNo { get; set; }
        public double Quantity { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
    }

    public class JournalEntries
    {
        public int JEId { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public string Source { get; set; }
        public string Reference { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public string Remarks { get; set; }
    }
    public class PageModel
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}