//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineInventory.DbContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class InvoiceReturnInfoDetailsTbl
    {
        public int ReturnRecordId { get; set; }
        public int ItemId { get; set; }
        public string ReturnInvoiceNo { get; set; }
        public double Quantity { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }
    }
}
