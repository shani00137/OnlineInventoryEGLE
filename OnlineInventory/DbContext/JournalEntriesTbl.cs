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
    
    public partial class JournalEntriesTbl
    {
        public int JEId { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public string Source { get; set; }
        public string Reference { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
