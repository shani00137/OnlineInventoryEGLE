using OnlineInventory.DbContext;
using OnlineInventory.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace OnlineInventory.DAL
{
    public class ReportsDB
    {
        public static DataTable GetInvoiceReport(String Id)
        {
            List<SaleInvoiceModel> list = new List<SaleInvoiceModel>();
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {

                String InvId = string.Join("", Id.ToCharArray().Where(Char.IsDigit));



                DataTable ddt = new DataTable();
                ddt.Columns.Add("InvoiceNo");
                ddt.Columns.Add("InvoiceDate", typeof(DateTime));
                ddt.Columns.Add("CustomerId");
                ddt.Columns.Add("Name");
                ddt.Columns.Add("Mobile");
                ddt.Columns.Add("Phone");
                ddt.Columns.Add("Address");
                ddt.Columns.Add("ItemNo");
                ddt.Columns.Add("ItemName");
                ddt.Columns.Add("Quantity", typeof(double));
                ddt.Columns.Add("Rate", typeof(double));
                ddt.Columns.Add("Amount", typeof(double));

                var Query = (from c in dbContaxt.InvoiceInfoTbls
                             join d in dbContaxt.InvoiceInfoDetailTbls on c.InvoiceNo equals d.InvoiceNo
                             join i in dbContaxt.ItemsTbls on d.ItemId equals i.ItemId
                             join a in dbContaxt.CustomerTbls on c.CustomerId equals a.CustomerId
                             where c.InvoiceNo == Id
                             select new
                             {
                                 c.InvoiceDate,
                                 c.CustomerId,
                                 c.InvoiceNo,
                                 d.ItemId,
                                 d.Quantity,
                                 d.Amount,
                                 d.Rate,
                                 i.ItemName,
                                 a.Name,
                                 a.Phone,
                                 a.Mobile,
                                 a.Address,
                                 c.Remarks
                             }).ToList();
                foreach (var q in Query)
                {
                    ddt.Rows.Add(q.InvoiceNo, q.InvoiceDate, q.CustomerId, q.Name, q.Mobile, q.Phone, q.Address, q.ItemId, q.ItemName, q.Quantity, q.Rate, q.Amount);
                }
                return ddt;

            }
        }
        public static DataTable GetGeneralLedgerReport(int CustomerID, DateTime FromDate, DateTime ToDate)
        {
            List<SaleInvoiceModel> list = new List<SaleInvoiceModel>();

            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {


                DataTable ddt = new DataTable();
                ddt.Columns.Add("InvoiceNo");
                ddt.Columns.Add("InvoiceDate", typeof(DateTime));
                ddt.Columns.Add("Name");
                ddt.Columns.Add("ItemId");
                ddt.Columns.Add("ItemName");
                ddt.Columns.Add("Quantity", typeof(int));
                ddt.Columns.Add("Rate", typeof(double));
                ddt.Columns.Add("Debit", typeof(double));
                ddt.Columns.Add("Credit", typeof(double));
                ddt.Columns.Add("Balance", typeof(double));
                ddt.Columns.Add("OpeningBalance", typeof(double));
                ddt.Columns.Add("Duration");
                ddt.Columns.Add("Source");
                var CustomerInfo = dbContaxt.CustomerTbls.Where(x => x.CustomerId == CustomerID).FirstOrDefault();
                var Query = (from c in dbContaxt.JournalEntriesTbls
                             where c.Reference == CustomerInfo.CustomerNo && c.TransactionDate >= FromDate && c.TransactionDate <= ToDate
                             select new
                             {
                                 c.Source,
                                 c.Reference,
                                 c.JEId,
                                 c.Remarks,
                                 c.Debit,
                                 c.Credit,
                                 c.TransactionDate
                             }).OrderBy(x => x.TransactionDate).ToList();


                String Duration = FromDate.ToString("dd-MMM-yyyy") + " To " + ToDate.ToString("dd-MMM-yyyy");
                double? OpeningBalanceAmount = 0;
                OpeningBalanceAmount = (dbContaxt.CustomerTbls.Where(x => x.CustomerId == CustomerID).ToList()).Select(x => x.OpeningBal).Sum();
                var OpeningBalance = dbContaxt.JournalEntriesTbls.Where(x => x.Reference == CustomerInfo.CustomerNo && x.TransactionDate < FromDate).ToList();
                
                if (OpeningBalance.Count > 0)
                {
                    double Debit = OpeningBalance.Select(x => x.Debit).Sum();
                    double Credit = OpeningBalance.Select(x => x.Credit).Sum();
                    OpeningBalanceAmount = OpeningBalanceAmount + (Debit - Credit);

                }
                if (Query.Count > 0)
                {
                    foreach (var q in Query)
                    {


                        double CurrentBalance = 0;
                        var CurrentBalanceQuery = Query.Where(x => x.JEId <= q.JEId).ToList();
                        if (CurrentBalanceQuery.Count > 0)
                        {
                            double Debit = (double)CurrentBalanceQuery.Select(x => x.Debit).Sum();
                            double Credit = (double)CurrentBalanceQuery.Select(x => x.Credit).Sum();
                            CurrentBalance = Debit - Credit;
                        }
                        var SaleQuery = (from c in dbContaxt.InvoiceInfoDetailTbls
                                         join s in dbContaxt.ItemsTbls on c.ItemId equals s.ItemId
                                         where c.InvoiceNo == q.Source
                                         select new
                                         {
                                             c.ItemId,
                                             c.Rate,
                                             c.Quantity,
                                             s.ItemName
                                         }).ToList();

                        //ddt.Rows.Add(q.InvoiceNo, q.InvoiceDate, q.CustomerId, q.Name, q.Mobile, q.Phone, q.Address,0, "", 0, 0, q.Debit, q.Credit, CurrentBalance, OpeningBalanceAmount);
                        if (SaleQuery.Count > 0)
                        {
                            foreach (var s in SaleQuery)
                            {
                                ddt.Rows.Add(q.Source, q.TransactionDate, CustomerInfo.Name, s.ItemId, s.ItemName, s.Quantity, s.Rate, q.Debit, q.Credit, CurrentBalance, OpeningBalanceAmount, Duration, q.Source);
                            }

                        }
                        else
                        {
                            ddt.Rows.Add(q.Source, q.TransactionDate, CustomerInfo.Name, "", "", 0, 0, q.Debit, q.Credit, CurrentBalance, OpeningBalanceAmount, Duration, q.Source);
                        }


                    }
                    //foreach (var m in Query1)
                    //{
                    //    ddt.Rows.Add("", m.TransactionDate, m.Source, "", "", "", "", 1, "",0, 0, m.Debit, m.Credit, 0, OpeningBalanceAmount, Duration, m.Source);
                    //}
                }
                else
                {
                    //add Opening Balance Only
                    //ddt.Rows.Add(0, DateTime.Now, CustomerID, CustomerInfo.Name, CustomerInfo.Mobile, CustomerInfo.Phone, CustomerInfo.Address, 0, "Opening Balance", 0, 0, 0, 0, OpeningBalanceAmount, OpeningBalanceAmount, Duration);
                }
                return ddt;

            }
        }
        public static List<SaleInvoiceModel> GetGeneralLedgerReportViwer(int CustomerID, DateTime FromDate, DateTime ToDate)
        {
            List<SaleInvoiceModel> list = new List<SaleInvoiceModel>();

            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {


                var CustomerInfo = dbContaxt.CustomerTbls.Where(x => x.CustomerId == CustomerID).FirstOrDefault();
                var Query = (from c in dbContaxt.JournalEntriesTbls
                             join a in dbContaxt.CustomerTbls on c.Reference equals a.CustomerNo
                             where c.Reference == CustomerInfo.CustomerNo && c.TransactionDate >= FromDate && c.TransactionDate <= ToDate
                             select new
                             {
                                 c.Debit,
                                 c.Credit,
                                 c.Source,
                                 c.CreatedDate,
                                 c.Reference,
                                 c.JEId,
                                 a.Name,
                                 a.Phone,
                                 a.Mobile,
                                 a.Address,
                                 c.Remarks,
                                 c.TransactionDate,
                             }).ToList();





                String Duration = FromDate.ToString("dd-MMM-yyyy") + " TO " + ToDate.ToString("dd-MMM-yyyy");
                var OpeningBalance = dbContaxt.JournalEntriesTbls.Where(x => x.Reference == CustomerInfo.CustomerNo && x.TransactionDate < FromDate).ToList();
                double? OpeningBalanceAmount = 0;
                OpeningBalanceAmount = (dbContaxt.CustomerTbls.Where(x => x.CustomerId == CustomerID).ToList()).Select(x => x.OpeningBal).Sum();

                if (OpeningBalance.Count > 0)
                {
                    double Debit = OpeningBalance.Select(x => x.Debit).Sum();
                    double Credit = OpeningBalance.Select(x => x.Credit).Sum();
                    OpeningBalanceAmount = OpeningBalanceAmount + (Debit - Credit);

                }

                foreach (var q in Query)
                {
                    list.Add(new SaleInvoiceModel
                    {
                        Reference = q.Reference,
                        Name = q.Name,
                        CreatedDate = q.CreatedDate,
                        Debit = q.Debit,
                        Credit = q.Credit,
                        InvoiceNo = q.Source,
                        InvoiceDate = Convert.ToDateTime(q.TransactionDate),
                        OpeningBalance = OpeningBalanceAmount
                    });
                    //ddt.Rows.Add(q.Reference,q.CreatedDate,q.Name,q.Debit,q.Credit,0);

                }


                return list;

            }
        }
        public static List<SaleInvoiceModel> GetGeneralLedgerReportAllViwer(int CustomerID)
        {
            List<SaleInvoiceModel> list = new List<SaleInvoiceModel>();

            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {


                var CustomerInfo = dbContaxt.CustomerTbls.Where(x => x.CustomerId == CustomerID).FirstOrDefault();
                var Query = (from c in dbContaxt.JournalEntriesTbls
                             join a in dbContaxt.CustomerTbls on c.Reference equals a.CustomerNo
                             where c.Reference == CustomerInfo.CustomerNo
                             select new
                             {
                                 c.Debit,
                                 c.Credit,
                                 c.Source,
                                 c.CreatedDate,
                                 c.Reference,
                                 c.JEId,
                                 a.Name,
                                 a.Phone,
                                 a.Mobile,
                                 a.Address,
                                 c.Remarks,
                             }).ToList();





                //String Duration = FromDate.ToString("dd-MMM-yyyy") + " TO " + ToDate.ToString("dd-MMM-yyyy");
                //var OpeningBalance = dbContaxt.JournalEntriesTbls.Where(x => x.Reference == CustomerInfo.CustomerNo && x.TransactionDate < FromDate).ToList();
                double OpeningBalanceAmount = 0;
                //if (OpeningBalance.Count > 0)
                //{
                //    double Debit = OpeningBalance.Select(x => x.Debit).Sum();
                //    double Credit = OpeningBalance.Select(x => x.Credit).Sum();
                //    OpeningBalanceAmount = Debit - Credit;

                //}
                SaleInvoiceModel obj = new SaleInvoiceModel();
                obj.Reference = "Opening Bal.";
                obj.Name = CustomerInfo.Name;
                obj.CreatedDate = CustomerInfo.CreatedDate;
                obj.Debit = Convert.ToDouble(CustomerInfo.OpeningBal);
                obj.Credit = 0;
                obj.InvoiceNo = "Opening Bal.";
                obj.OpeningBalance = 0;
                list.Add(obj);
                foreach (var q in Query)
                {
                    list.Add(new SaleInvoiceModel
                    {
                        Reference = q.Reference,
                        Name = q.Name,
                        CreatedDate = q.CreatedDate,
                        Debit = q.Debit,
                        Credit = q.Credit,
                        InvoiceNo = q.Source,
                        OpeningBalance = OpeningBalanceAmount
                    });
                    //ddt.Rows.Add(q.Reference,q.CreatedDate,q.Name,q.Debit,q.Credit,0);

                }


                return list;

            }
        }

        public static DataTable GetItemReportByCustomer(int Id)
        {
            List<SaleInvoiceModel> list = new List<SaleInvoiceModel>();
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {


                DataTable ddt = new DataTable();

                ddt.Columns.Add("ItemName");
                ddt.Columns.Add("Rate", typeof(double));
                ddt.Columns.Add("Name");
                var Query = (from c in dbContaxt.ItemsTbls
                             join s in dbContaxt.CustomerTbls on c.CustomerId equals s.CustomerId
                             where c.CustomerId == Id
                             select new
                             {
                                 c.ItemName,
                                 c.SaleRate,
                                 s.Name
                             }).ToList();
                foreach (var q in Query)
                {
                    ddt.Rows.Add(q.ItemName, q.SaleRate, q.Name);
                }
                return ddt;

            }
        }

    }
}