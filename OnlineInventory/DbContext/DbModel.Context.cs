﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class OnlineInvoiceSystemDBEntities : DbContext
    {
        public OnlineInvoiceSystemDBEntities()
            : base("name=OnlineInvoiceSystemDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CustomerTbl> CustomerTbls { get; set; }
        public virtual DbSet<InvoiceInfoDetailTbl> InvoiceInfoDetailTbls { get; set; }
        public virtual DbSet<InvoiceInfoTbl> InvoiceInfoTbls { get; set; }
        public virtual DbSet<InvoiceReturnInfoDetailsTbl> InvoiceReturnInfoDetailsTbls { get; set; }
        public virtual DbSet<ItemsTbl> ItemsTbls { get; set; }
        public virtual DbSet<JournalEntriesTbl> JournalEntriesTbls { get; set; }
        public virtual DbSet<ReceiptVoucher> ReceiptVouchers { get; set; }
        public virtual DbSet<ReturnInvoiceInfoTbl> ReturnInvoiceInfoTbls { get; set; }
        public virtual DbSet<UserLoginTbl> UserLoginTbls { get; set; }
    
        public virtual ObjectResult<Sp_CustomerLedger_Result> Sp_CustomerLedger(string customerNo, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate)
        {
            var customerNoParameter = customerNo != null ?
                new ObjectParameter("CustomerNo", customerNo) :
                new ObjectParameter("CustomerNo", typeof(string));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_CustomerLedger_Result>("Sp_CustomerLedger", customerNoParameter, fromDateParameter, toDateParameter);
        }
    }
}
