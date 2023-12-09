using OnlineInventory.DbContext;
using OnlineInventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineInventory.DAL
{
    public class SalesDb
    {
        public static List<InvoiceInfoMD> GetAllInvoices(int? CustomerID, DateTime? FromDate, DateTime? ToDate,string InvoiceNo)
        {
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {
                List<InvoiceInfoMD> list = new List<InvoiceInfoMD>();
                var Query = (from c in dbContaxt.InvoiceInfoTbls
                             join m in dbContaxt.CustomerTbls                            
                             on c.CustomerId equals m.CustomerId
                             join u in dbContaxt.UserLoginTbls
                             on c.CreatedBy equals u.LoginId
                             where (CustomerID == null || m.CustomerId == CustomerID) &&
                            (FromDate == null || c.InvoiceDate >= FromDate) &&
                            (ToDate == null || c.InvoiceDate <= ToDate) &&
                   (string.IsNullOrEmpty(InvoiceNo) || c.InvoiceNo.Contains(InvoiceNo.Trim()))

                             select new
                             {
                                 c.CreatedDate,
                                 c.InvoiceDate,
                                 c.InvoiceNo,
                                 c.Remarks,
                                 c.CustomerId,
                                 m.Name,
                                 c.RecordId,
                                 u.UserName
                             }).OrderByDescending(x => x.RecordId).ToList();
                foreach (var q in Query)
                {
                    list.Add(new InvoiceInfoMD
                    {
                        CustomerId = q.CustomerId,
                        InvoiceDate = Convert.ToDateTime(q.InvoiceDate).Date,
                        Remarks = q.Remarks == null ? "" : q.Remarks,
                        InvoiceNo = q.InvoiceNo,
                        Name = q.Name,
                        CreatedBy = q.UserName,
                        CreatedDate = q.CreatedDate,
                        TotalAmount = dbContaxt.InvoiceInfoDetailTbls.Where(x => x.InvoiceNo == q.InvoiceNo).Select(x => x.Amount).Sum()

                    });
                }
                return list;

            }
        }
        public static List<InvoiceInfoMD> GetInvoiceInfo(string InvNo)
        {
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {
                List<InvoiceInfoMD> list = new List<InvoiceInfoMD>();
                var Query = (from c in dbContaxt.InvoiceInfoTbls
                             join m in dbContaxt.CustomerTbls
                             on c.CustomerId equals m.CustomerId
                             join u in dbContaxt.UserLoginTbls
                             on c.CreatedBy equals u.LoginId
                             where c.InvoiceNo == InvNo
                             select new
                             {
                                 c.CreatedDate,
                                 c.InvoiceDate,
                                 c.InvoiceNo,
                                 c.Remarks,
                                 c.CustomerId,
                                 m.Name,
                                 c.RecordId,
                                 u.UserName
                             }).OrderByDescending(x => x.RecordId).ToList();
                foreach (var q in Query)
                {
                    list.Add(new InvoiceInfoMD
                    {
                        CustomerId = q.CustomerId,
                        InvoiceDate = Convert.ToDateTime(q.InvoiceDate).Date,
                        Remarks = q.Remarks == null ? "" : q.Remarks,
                        InvoiceNo = q.InvoiceNo,
                        Name = q.Name,
                        CreatedBy = q.UserName,
                        CreatedDate = q.CreatedDate

                    });
                }
                return list;

            }
        }
        public static List<InvoiceInfoMD> GetAllReturnInvoice()
        {
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {
                List<InvoiceInfoMD> list = new List<InvoiceInfoMD>();
                var Query = (from c in dbContaxt.ReturnInvoiceInfoTbls
                             join m in dbContaxt.CustomerTbls
                             on c.CustomerId equals m.CustomerId
                             join u in dbContaxt.UserLoginTbls
                             on c.CreatedBy equals u.LoginId
                             select new
                             {
                                 c.CreatedDate,
                                 c.InvoiceDate,
                                 c.ReturnInvoiceNo,
                                 c.Remarks,
                                 c.CustomerId,
                                 m.Name,
                                 c.ReturnRecordId,
                                 u.UserName
                             }).OrderByDescending(x => x.ReturnRecordId).ToList();
                foreach (var q in Query)
                {
                    list.Add(new InvoiceInfoMD
                    {
                        CustomerId = q.CustomerId,
                        InvoiceDate = Convert.ToDateTime(q.InvoiceDate).Date,
                        Remarks = q.Remarks,
                        ReturnInvoiceNo = q.ReturnInvoiceNo,
                        Name = q.Name,
                        CreatedBy = q.UserName,
                        CreatedDate = q.CreatedDate

                    });
                }
                return list;

            }
        }
        public static List<InvoiceDetailMD> GetReturnInvoiceDetails(String InvoiceNo)
        {
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {
                List<InvoiceDetailMD> list = new List<InvoiceDetailMD>();
                var Query = (from c in dbContaxt.InvoiceReturnInfoDetailsTbls
                             join m in dbContaxt.ItemsTbls
                             on c.ItemId equals m.ItemId
                             where c.ReturnInvoiceNo == InvoiceNo
                             select new
                             {
                                 c.ItemId,
                                 c.Amount,
                                 c.Rate,
                                 c.Quantity,
                                 m.ItemCode,
                                 m.ItemName
                             }).ToList();
                foreach (var q in Query)
                {
                    list.Add(new InvoiceDetailMD
                    {
                        ItemId = q.ItemId,
                        Rate = q.Rate,
                        Amount = q.Amount,
                        Quantity = q.Quantity,
                        ItemName = q.ItemName,
                        ItemCode = q.ItemCode

                    });
                }
                return list;

            }
        }
        public static String DeleteSaleInvoice(String InvoiceNo)
        {
            var response = "";
            try
            {
                using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
                {
                    //delete Sale details
                    var SaleQuery = dbContaxt.InvoiceInfoTbls.Where(x => x.InvoiceNo == InvoiceNo).FirstOrDefault();
                    if (SaleQuery != null)
                    {
                        dbContaxt.InvoiceInfoTbls.Remove(SaleQuery);
                        dbContaxt.SaveChanges();
                    }
                    //Delete Invoice Details
                    var SaleDetailsQuery = dbContaxt.InvoiceInfoDetailTbls.Where(x => x.InvoiceNo == InvoiceNo).ToList();
                    if (SaleDetailsQuery.Count > 0)
                    {
                        foreach (var q in SaleDetailsQuery)
                        {
                            var DeleteDetails = dbContaxt.InvoiceInfoDetailTbls.Where(x => x.RecordId == q.RecordId).FirstOrDefault();
                            dbContaxt.InvoiceInfoDetailTbls.Remove(DeleteDetails);
                            dbContaxt.SaveChanges();

                        }
                    }
                    //delete Journey ledger entry
                    var Jquery = dbContaxt.JournalEntriesTbls.Where(x => x.Reference == InvoiceNo).FirstOrDefault();
                    if (Jquery != null)
                    {
                        dbContaxt.JournalEntriesTbls.Remove(Jquery);
                        dbContaxt.SaveChanges();
                    }
                    response = "Deleted Successfully";
                }
            }
            catch (Exception ex)
            {
                response = ex.ToString();
                throw;
            }
            return response;
        }
        public static String DeleteReturnSaleInvoice(String InvoiceNo)
        {
            var response = "";
            try
            {
                using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
                {
                    //delete Sale details
                    var SaleQuery = dbContaxt.ReturnInvoiceInfoTbls.Where(x => x.ReturnInvoiceNo == InvoiceNo).FirstOrDefault();
                    if (SaleQuery != null)
                    {
                        dbContaxt.ReturnInvoiceInfoTbls.Remove(SaleQuery);
                        dbContaxt.SaveChanges();
                    }
                    //Delete Invoice Details
                    var SaleDetailsQuery = dbContaxt.InvoiceReturnInfoDetailsTbls.Where(x => x.ReturnInvoiceNo == InvoiceNo).ToList();
                    if (SaleDetailsQuery.Count > 0)
                    {
                        foreach (var q in SaleDetailsQuery)
                        {
                            var DeleteDetails = dbContaxt.InvoiceReturnInfoDetailsTbls.Where(x => x.ReturnRecordId == q.ReturnRecordId).FirstOrDefault();
                            dbContaxt.InvoiceReturnInfoDetailsTbls.Remove(DeleteDetails);
                            dbContaxt.SaveChanges();

                        }
                    }
                    //delete Journey ledger entry
                    var Jquery = dbContaxt.JournalEntriesTbls.Where(x => x.Reference == InvoiceNo).FirstOrDefault();
                    if (Jquery != null)
                    {
                        dbContaxt.JournalEntriesTbls.Remove(Jquery);
                        dbContaxt.SaveChanges();
                    }
                    response = "Deleted Successfully";
                }
            }
            catch (Exception ex)
            {
                response = ex.ToString();
                throw;
            }
            return response;
        }
        public static String AddEditSaleInvoice(SaleInvoiceModel obj)
        {
            String InvoiceNo = "";
            try
            {
                using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
                {

                    if (obj.InvoiceNo == null)
                    {
                        int MaxId = 1;
                        //var GetMaxId = dbContaxt.InvoiceInfoTbls.OrderByDescending(x => x.InvoiceNo).FirstOrDefault();

                        var GetMaxId = (from c in dbContaxt.InvoiceInfoTbls
                                        where c.InvoiceNo.Contains("INV")
                                        select c).Max(c => c.InvoiceNo);

                        if (GetMaxId != null)
                        {
                            var MaxRecId = (from c in dbContaxt.InvoiceInfoTbls
                                            where c.InvoiceNo.Contains("INV")
                                            select c).Max(c => c.RecordId);

                            String OrderNo = MaxRecId.ToString();
                            //MaxId =  1 + int.Parse(OrderNo.Substring(3, OrderNo.Length - 3));
                            MaxId = int.Parse(OrderNo) + 1;
                        }
                        

                        InvoiceInfoTbl objInvoice = new InvoiceInfoTbl();
                        objInvoice.InvoiceNo = "INV" + MaxId;
                        objInvoice.CustomerId = obj.CustomerId;
                        objInvoice.InvoiceDate = obj.InvoiceDate;
                        objInvoice.Remarks = obj.Remarks;
                        objInvoice.CreatedBy = obj.CreatedBy;
                        objInvoice.CreatedDate = DateTime.Now;
                        dbContaxt.InvoiceInfoTbls.Add(objInvoice);

                        foreach (var q in obj.CartList)
                        {
                            InvoiceInfoDetailTbl invoiceInfoMD = new InvoiceInfoDetailTbl();
                            invoiceInfoMD.InvoiceNo = "INV" + MaxId;
                            invoiceInfoMD.ItemId = q.ItemId;
                            invoiceInfoMD.Quantity = q.Quantity;
                            invoiceInfoMD.Rate = q.Rate;
                            invoiceInfoMD.Amount = q.Amount;
                            dbContaxt.InvoiceInfoDetailTbls.Add(invoiceInfoMD);
                        }
                        dbContaxt.SaveChanges();

                        var CustomerNo = dbContaxt.CustomerTbls.Where(x => x.CustomerId == obj.CustomerId).Select(x => x.CustomerNo).FirstOrDefault();
                        JournalEntriesTbl ObjJournalEntries = new JournalEntriesTbl();
                        ObjJournalEntries.TransactionDate = Convert.ToDateTime(obj.InvoiceDate);
                        ObjJournalEntries.Source = "INV" + MaxId;
                        ObjJournalEntries.Reference = CustomerNo.ToString();
                        ObjJournalEntries.Debit = obj.CartList.Select(x => x.Amount).Sum();
                        ObjJournalEntries.Credit = 0;
                        ObjJournalEntries.Remarks = obj.Remarks;
                        ObjJournalEntries.CreatedBy = 1;
                        ObjJournalEntries.CreatedDate = DateTime.Now;

                        dbContaxt.JournalEntriesTbls.Add(ObjJournalEntries);
                        dbContaxt.SaveChanges();
                        InvoiceNo = "INV" + MaxId;
                    }
                    else
                    {
                        InvoiceNo = obj.InvoiceNo;


                        var SaleInfo = dbContaxt.InvoiceInfoDetailTbls.Where(x => x.InvoiceNo == obj.InvoiceNo).ToList();
                        foreach (var q in SaleInfo)
                        {
                            var Delete = dbContaxt.InvoiceInfoDetailTbls.Where(x => x.RecordId == q.RecordId).FirstOrDefault();
                            dbContaxt.InvoiceInfoDetailTbls.Remove(Delete);

                        }
                        foreach (var q in obj.CartList)
                        {
                            InvoiceInfoDetailTbl invoiceInfoMD = new InvoiceInfoDetailTbl();
                            invoiceInfoMD.InvoiceNo = obj.InvoiceNo;
                            invoiceInfoMD.ItemId = q.ItemId;
                            invoiceInfoMD.Quantity = q.Quantity;
                            invoiceInfoMD.Rate = q.Rate;
                            invoiceInfoMD.Amount = q.Amount;
                            dbContaxt.InvoiceInfoDetailTbls.Add(invoiceInfoMD);
                        }

                        //////////////////////////////////SALE INFO UPDATE////////////////////////////////
                        InvoiceInfoTbl objSaleInfo = dbContaxt.InvoiceInfoTbls.Where(x => x.InvoiceNo == InvoiceNo).FirstOrDefault();
                        objSaleInfo.CustomerId = obj.CustomerId;
                        objSaleInfo.InvoiceDate = obj.InvoiceDate;
                        objSaleInfo.Remarks = obj.Remarks;
                        dbContaxt.SaveChanges();
                        /////////////////////////////////////////////////////////////////
                        var CustomerNo = dbContaxt.CustomerTbls.Where(x => x.CustomerId == obj.CustomerId).Select(x => x.CustomerNo).FirstOrDefault();
                        // Update Journal Ledger
                        var JournalQuery = dbContaxt.JournalEntriesTbls.Where(x => x.Source == obj.InvoiceNo).FirstOrDefault();
                        //JournalEntriesTbl ObjJournalEntries = dbContaxt.JournalEntriesTbls.Where(a => a.Source == obj.InvoiceNo.ToString()).FirstOrDefault();
                        JournalQuery.TransactionDate = Convert.ToDateTime(obj.InvoiceDate);
                        JournalQuery.Source = InvoiceNo;
                        JournalQuery.Reference = CustomerNo.ToString();
                        JournalQuery.Debit = obj.CartList.Select(x => x.Amount).Sum(); ;
                        JournalQuery.Credit = 0;
                        JournalQuery.Remarks = obj.Remarks;
                        JournalQuery.CreatedBy = 1;
                        JournalQuery.CreatedDate = DateTime.Now;
                        dbContaxt.SaveChanges();
                    }


                }
                return InvoiceNo;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

        public static String AddEditReturnSaleInvoice(SaleInvoiceModel obj)
        {
            String InvoiceNo = "";
            try
            {
                using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
                {

                    if (obj.ReturnInvoiceNo == null)
                    {
                        int MaxId = 1;
                        var GetMaxId = dbContaxt.ReturnInvoiceInfoTbls.OrderByDescending(x => x.ReturnRecordId).FirstOrDefault();
                        if (GetMaxId != null)
                        {
                            String OrderNo = GetMaxId.ReturnInvoiceNo.ToString();
                            MaxId = 1 + int.Parse(OrderNo.Replace("RINV", ""));
                        }

                        ReturnInvoiceInfoTbl objInvoice = new ReturnInvoiceInfoTbl();
                        objInvoice.ReturnInvoiceNo = "RINV" + MaxId;
                        objInvoice.CustomerId = obj.CustomerId;
                        objInvoice.InvoiceDate = obj.InvoiceDate;
                        objInvoice.Remarks = obj.Remarks;
                        objInvoice.CreatedBy = 1;
                        objInvoice.CreatedDate = DateTime.Now;
                        dbContaxt.ReturnInvoiceInfoTbls.Add(objInvoice);

                        foreach (var q in obj.CartList)
                        {
                            InvoiceReturnInfoDetailsTbl invoiceInfoMD = new InvoiceReturnInfoDetailsTbl();
                            invoiceInfoMD.ReturnInvoiceNo = "RINV" + MaxId;
                            invoiceInfoMD.ItemId = q.ItemId;
                            invoiceInfoMD.Quantity = q.Quantity;
                            invoiceInfoMD.Rate = q.Rate;
                            invoiceInfoMD.Amount = q.Amount;
                            dbContaxt.InvoiceReturnInfoDetailsTbls.Add(invoiceInfoMD);
                        }
                        dbContaxt.SaveChanges();

                        var CustomerNo = dbContaxt.CustomerTbls.Where(x => x.CustomerId == obj.CustomerId).Select(x => x.CustomerNo).FirstOrDefault();
                        JournalEntriesTbl ObjJournalEntries = new JournalEntriesTbl();
                        ObjJournalEntries.TransactionDate = Convert.ToDateTime(obj.InvoiceDate);
                        ObjJournalEntries.Source = "RINV" + MaxId;
                        ObjJournalEntries.Reference = CustomerNo.ToString();
                        ObjJournalEntries.Debit = 0;
                        ObjJournalEntries.Credit = obj.CartList.Select(x => x.Amount).Sum();
                        ObjJournalEntries.Remarks = obj.Remarks;
                        ObjJournalEntries.CreatedBy = 1;
                        ObjJournalEntries.CreatedDate = DateTime.Now;

                        dbContaxt.JournalEntriesTbls.Add(ObjJournalEntries);
                        dbContaxt.SaveChanges();
                        InvoiceNo = "RINV" + MaxId;
                    }
                    else
                    {
                        var SaleInfo = dbContaxt.InvoiceReturnInfoDetailsTbls.Where(x => x.ReturnInvoiceNo == obj.ReturnInvoiceNo).ToList();
                        foreach (var q in SaleInfo)
                        {
                            var Delete = dbContaxt.InvoiceReturnInfoDetailsTbls.Where(x => x.ReturnRecordId == q.ReturnRecordId).FirstOrDefault();
                            dbContaxt.InvoiceReturnInfoDetailsTbls.Remove(Delete);
                            dbContaxt.SaveChanges();
                           
                        }
                        foreach (var q in obj.CartList)
                        {
                            InvoiceReturnInfoDetailsTbl invoiceInfoMD = new InvoiceReturnInfoDetailsTbl();
                            invoiceInfoMD.ReturnInvoiceNo = obj.ReturnInvoiceNo;
                            invoiceInfoMD.ItemId = q.ItemId;
                            invoiceInfoMD.Quantity = q.Quantity;
                            invoiceInfoMD.Rate = q.Rate;
                            dbContaxt.InvoiceReturnInfoDetailsTbls.Add(invoiceInfoMD);
                        }
                        dbContaxt.SaveChanges();
                        InvoiceNo = obj.ReturnInvoiceNo;
                        JournalEntriesTbl ObjJournalEntries = dbContaxt.JournalEntriesTbls.Where(a => a.Source == InvoiceNo).FirstOrDefault();
                        ObjJournalEntries.TransactionDate = Convert.ToDateTime(obj.InvoiceDate);
                        ObjJournalEntries.Source = InvoiceNo;
                        ObjJournalEntries.Reference = obj.CustomerId.ToString();
                        ObjJournalEntries.Debit = 0;
                        ObjJournalEntries.Credit = obj.CartList.Select(x => x.Amount).Sum(); ; ;
                        ObjJournalEntries.Remarks = obj.Remarks;
                        ObjJournalEntries.CreatedBy = 1;
                        ObjJournalEntries.CreatedDate = DateTime.Now;
                        dbContaxt.SaveChanges();
                    }


                }
                return InvoiceNo;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }



        public static List<InvoiceInfoMD> LoadInvoiceByNo(string InvoiceNo)
        {
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {
                var query = (from invnfo in dbContaxt.InvoiceInfoTbls
                             join invDet in dbContaxt.InvoiceInfoDetailTbls on invnfo.InvoiceNo equals invDet.InvoiceNo
                             join itms in dbContaxt.ItemsTbls on invDet.ItemId equals itms.ItemId
                             where (invnfo.InvoiceNo == InvoiceNo)
                             select new
                             {
                                 InvoiceNo = invnfo.InvoiceNo,
                                 InvoiceDate = invnfo.InvoiceDate,
                                 CustomerId = invnfo.CustomerId,
                                 Quantity = invDet.Quantity,
                                 Rate = invDet.Rate,
                                 Amount = invDet.Amount,
                                 ItemId = invDet.ItemId,
                                 ItemName = itms.ItemName,
                             });

                var VoucherList = new List<InvoiceInfoMD>();
                foreach (var itm in query)
                {
                    VoucherList.Add(new InvoiceInfoMD()
                    {
                        InvoiceNo = itm.InvoiceNo,
                        InvoiceDate = itm.InvoiceDate,
                        CustomerId = itm.CustomerId,
                        Quantity = itm.Quantity,
                        Rate = itm.Rate,
                        Amount = itm.Amount,
                        ItemId = itm.ItemId,
                        ItemName = itm.ItemName,
                    });
                }
                return VoucherList;

            }
        }
        //Get Return Invoice 
        public static List<InvoiceInfoMD> LoadReturnInvoiceByNo(string InvoiceNo)
        {
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {
                var query = (from invnfo in dbContaxt.ReturnInvoiceInfoTbls
                             join invDet in dbContaxt.InvoiceReturnInfoDetailsTbls on invnfo.ReturnInvoiceNo equals invDet.ReturnInvoiceNo
                             join itms in dbContaxt.ItemsTbls on invDet.ItemId equals itms.ItemId
                             where (invnfo.ReturnInvoiceNo == InvoiceNo)
                             select new
                             {
                                 ReturnInvoiceNo = invnfo.ReturnInvoiceNo,
                                 InvoiceDate = invnfo.InvoiceDate,
                                 CustomerId = invnfo.CustomerId,
                                 Quantity = invDet.Quantity,
                                 Rate = invDet.Rate,
                                 Amount = invDet.Amount,
                                 ItemId = invDet.ItemId,
                                 ItemName = itms.ItemName,
                             });

                var VoucherList = new List<InvoiceInfoMD>();
                foreach (var itm in query)
                {
                    VoucherList.Add(new InvoiceInfoMD()
                    {
                        InvoiceNo = itm.ReturnInvoiceNo,
                        InvoiceDate = itm.InvoiceDate,
                        CustomerId = itm.CustomerId,
                        Quantity = itm.Quantity,
                        Rate = itm.Rate,
                        Amount = itm.Amount,
                        ItemId = itm.ItemId,
                        ItemName = itm.ItemName,
                    });
                }
                return VoucherList;

            }
        }

        public static List<InvoiceInfoMD> LoadCustomerBalances()
        {
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {
                var customers = (from Customerss in dbContaxt.CustomerTbls
                                 select new
                                 {
                                     CustomerId = Customerss.CustomerId,
                                     CustomerNo = Customerss.CustomerNo,
                                     Name = Customerss.Name,
                                     OpeningBal = Customerss.OpeningBal
                                 });
                double Dr = 0;
                double Cr = 0;
                double Bal = 0;
                var CustomersBalances = new List<InvoiceInfoMD>();
                foreach (var cust in customers)
                {
                    Dr = (double)dbContaxt.JournalEntriesTbls.Where(x => x.Reference == cust.CustomerNo).Select(x => x.Debit).DefaultIfEmpty(0).Sum();
                    Cr = (double)dbContaxt.JournalEntriesTbls.Where(x => x.Reference == cust.CustomerNo).Select(x => x.Credit).DefaultIfEmpty(0).Sum();
                    Bal = Convert.ToDouble(cust.OpeningBal) +  (Dr - Cr);

                    CustomersBalances.Add(new InvoiceInfoMD()
                    {
                        CustomerId = cust.CustomerId,
                        Name = cust.Name,
                        TotalAmount = Bal,
                    });
                }


                return CustomersBalances;

            }
        }
        //public static bool DeleteSaleInvoice(string InvNo)
        //{
        //    bool Status = false;
        //    using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
        //    {
        //        try
        //        {
        //            var InvoiceDetail = dbContaxt.InvoiceInfoTbls.Where(x => x.InvoiceNo == InvNo).FirstOrDefault();
        //            dbContaxt.InvoiceInfoTbls.Remove(InvoiceDetail);
        //            dbContaxt.SaveChanges();
        //            var InvoiceDetailsQuery = dbContaxt.InvoiceInfoDetailTbls.Where(x => x.InvoiceNo == InvNo).ToList();
        //            foreach (var q in InvoiceDetailsQuery)
        //            {
        //                var Delete = dbContaxt.InvoiceInfoDetailTbls.Where(x => x.RecordId == q.RecordId).FirstOrDefault();
        //                dbContaxt.InvoiceInfoDetailTbls.Remove(Delete);
        //                dbContaxt.SaveChanges();
        //                Status = true;

        //            }


        //        }
        //        catch (Exception ex)
        //        {
        //            Status = false;
        //        }
        //        return Status;

        //    }
        //}
    }
}