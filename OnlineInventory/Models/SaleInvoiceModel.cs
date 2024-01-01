using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineInventory.DbContext;

namespace OnlineInventory.Models
{
    public class SaleInvoiceModel
    {
        public String InvoiceNo { get; set; }
        public String ReturnInvoiceNo { get; set; }
        
        public System.DateTime InvoiceDate { get; set; }
        public int CustomerId { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int ItemId { get; set; }
    
        public double Quantity { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public IEnumerable<SaleInvoiceDetailModel> CartList{ get; set; }
        public string ItemName { get;  set; }
        public string Reference { get;  set; }
        public Nullable<double>  OpeningBalance { get;  set; }
        public Nullable<double> Balance { get; set; }
    }
    public class SaleInvoiceDetailModel
    {
        public int ItemId { get; set; }
        public int InvoiceNo { get; set; }
        public double Quantity { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }
    }
}