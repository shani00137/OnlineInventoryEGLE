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
    
    public partial class Sp_CustomerLedger_Result
    {
        public string CustomerName { get; set; }
        public Nullable<System.DateTime> LedgerDate { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public Nullable<double> Debit { get; set; }
        public Nullable<double> Credit { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string Source { get; set; }
    }
}
