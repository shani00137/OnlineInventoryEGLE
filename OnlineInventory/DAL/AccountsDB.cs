using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineInventory.Models;
using OnlineInventory.DbContext;
using System.Data;
using System.ComponentModel;

namespace OnlineInventory.Models
{
    public class AccountsDB
    {
        public static string GenerateVoucherNo()
        {
            string VoucherNo = "";
            string Prefix = "RV-" + DateTime.Now.Date.Year.ToString().Substring(2, 2) + (DateTime.Now.Date.Month < 10 ? "0" + DateTime.Now.Date.Month.ToString() : DateTime.Now.Date.Month.ToString()) + "-";

            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {
                var max_Val = (from c in dbContaxt.ReceiptVouchers
                               select c).Max(c => c.VoucherNo);
                if (max_Val != null)
                    max_Val = max_Val.Substring(max_Val.Length - 4);
                max_Val = (Convert.ToInt64(max_Val) + 1).ToString();
                max_Val = "000" + max_Val;
                if (max_Val == null || max_Val == "1")
                    VoucherNo = Prefix + "0001";
                else
                    VoucherNo = Prefix + max_Val.Substring(max_Val.Length - 4);
            }
            return VoucherNo;
        }
        public static bool AddEditReceipt(ReceiptVoucherMD obj)
        {
            try
            {
                using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
                {
                    if (obj.CustomerID != 0)
                    {
                        if (obj.VoucherID == 0)
                        {
                            String CustomerCode = dbContaxt.CustomerTbls.Where(x => x.CustomerId == obj.CustomerID).Select(x => x.CustomerNo).FirstOrDefault();
                            ReceiptVoucher ObjReceiptVoucher = new ReceiptVoucher();
                            ObjReceiptVoucher.VoucherNo = obj.VoucherNo;
                            ObjReceiptVoucher.VoucherDate = Convert.ToDateTime(obj.VoucherDate);
                            ObjReceiptVoucher.CustomerID = obj.CustomerID;
                            ObjReceiptVoucher.CustomerNo = CustomerCode;
                            ObjReceiptVoucher.AmountReceived = obj.AmountReceived;
                            ObjReceiptVoucher.Remarks = obj.Remarks;
                            ObjReceiptVoucher.CreatedBy = 1;
                            ObjReceiptVoucher.CreatedDate = DateTime.Now;

                            dbContaxt.ReceiptVouchers.Add(ObjReceiptVoucher);
                            dbContaxt.SaveChanges();

                            JournalEntriesTbl ObjJournalEntries = new JournalEntriesTbl();
                            ObjJournalEntries.TransactionDate = Convert.ToDateTime(obj.VoucherDate);
                            ObjJournalEntries.Source = obj.VoucherNo;
                            ObjJournalEntries.Reference = CustomerCode;
                            ObjJournalEntries.Debit = 0;
                            ObjJournalEntries.Credit = obj.AmountReceived;
                            ObjJournalEntries.Remarks = obj.Remarks;
                            ObjJournalEntries.CreatedBy = 1;
                            ObjJournalEntries.CreatedDate = DateTime.Now;

                            dbContaxt.JournalEntriesTbls.Add(ObjJournalEntries);
                            dbContaxt.SaveChanges();
                        }
                        else
                        {
                            String CustomerCode = dbContaxt.CustomerTbls.Where(x => x.CustomerId == obj.CustomerID).Select(x => x.CustomerNo).FirstOrDefault();
                            ReceiptVoucher ObjReceiptVoucherEdit = dbContaxt.ReceiptVouchers.Where(a => a.VoucherID == obj.VoucherID).FirstOrDefault();

                            ObjReceiptVoucherEdit.VoucherNo = obj.VoucherNo;
                            ObjReceiptVoucherEdit.VoucherDate = Convert.ToDateTime(obj.VoucherDate);
                            ObjReceiptVoucherEdit.CustomerID = obj.CustomerID;
                            ObjReceiptVoucherEdit.AmountReceived = obj.AmountReceived;
                            ObjReceiptVoucherEdit.Remarks = obj.Remarks;
                            ObjReceiptVoucherEdit.CreatedBy = 1;
                            ObjReceiptVoucherEdit.CreatedDate = DateTime.Now;
                            dbContaxt.SaveChanges();

                            JournalEntriesTbl ObjJournalEntries = dbContaxt.JournalEntriesTbls.Where(a => a.Source == obj.VoucherNo).FirstOrDefault();
                            ObjJournalEntries.TransactionDate = Convert.ToDateTime(obj.VoucherDate);
                            ObjJournalEntries.Source = obj.VoucherNo;
                            ObjJournalEntries.Reference = CustomerCode;
                            ObjJournalEntries.Debit = 0;
                            ObjJournalEntries.Credit = obj.AmountReceived;
                            ObjJournalEntries.Remarks = obj.Remarks;
                            ObjJournalEntries.CreatedBy = 1;
                            ObjJournalEntries.CreatedDate = DateTime.Now;
                            dbContaxt.SaveChanges();
                        }
                    }
                    else
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public static List<ReceiptVoucherMD> GetVouchersList()
        {
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {
                var query = (from obj in dbContaxt.ReceiptVouchers
                             join Cust in dbContaxt.CustomerTbls on obj.CustomerID equals Cust.CustomerId
                             select new
                             {
                                 VoucherID = obj.VoucherID,
                                 VoucherNo = obj.VoucherNo,
                                 VoucherDate = obj.VoucherDate,
                                 CustomerID = obj.CustomerID,
                                 Name = Cust.Name,
                                 AmountReceived = obj.AmountReceived,
                                 Remarks = obj.Remarks,
                             });
                var VoucherList = new List<ReceiptVoucherMD>();
                foreach (var itm in query)
                {
                    VoucherList.Add(new ReceiptVoucherMD()
                    {
                        VoucherID = itm.VoucherID,
                        VoucherNo = itm.VoucherNo,
                        VoucherDate = itm.VoucherDate.ToString(),
                        CustomerID = Convert.ToInt32(itm.CustomerID),
                        CustName = itm.Name,
                        AmountReceived = Convert.ToInt32(itm.AmountReceived),
                        Remarks = itm.Remarks,
                    });
                }
                return VoucherList;

            }
        }

        public static List<ReceiptVoucherMD> GetVouchersListByID(int VoucherID)
        {
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {
                var query = (from obj in dbContaxt.ReceiptVouchers
                             join Cust in dbContaxt.CustomerTbls on obj.CustomerID equals Cust.CustomerId
                             where (obj.VoucherID == VoucherID)
                             select new
                             {
                                 VoucherID = obj.VoucherID,
                                 VoucherNo = obj.VoucherNo,
                                 VoucherDate = obj.VoucherDate,
                                 CustomerID = obj.CustomerID,
                                 Name = Cust.Name,
                                 AmountReceived = obj.AmountReceived,
                                 Remarks = obj.Remarks,
                             });

                var VoucherList = new List<ReceiptVoucherMD>();
                foreach (var itm in query)
                {
                    VoucherList.Add(new ReceiptVoucherMD()
                    {
                        VoucherID = itm.VoucherID,
                        VoucherNo = itm.VoucherNo,
                        VoucherDate = itm.VoucherDate.ToString(),
                        CustomerID = Convert.ToInt32(itm.CustomerID),
                        CustName = itm.Name,
                        AmountReceived = Convert.ToInt32(itm.AmountReceived),
                        Remarks = itm.Remarks,
                    });
                }
                return VoucherList;

            }
        }

        public static List<JournalEntriesMD> GetCustomerLedger(int CustomerId, DateTime FromDate, DateTime ToDate)
        {
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {
                String CustomerCode = dbContaxt.CustomerTbls.Where(x => x.CustomerId == CustomerId).Select(x => x.CustomerNo).FirstOrDefault();
                var query = dbContaxt.Sp_CustomerLedger(CustomerCode, FromDate, ToDate);

                var CustomerLedgerL = new List<JournalEntriesMD>();
                foreach (var SL in query)
                {
                    CustomerLedgerL.Add(new JournalEntriesMD()
                    {
                        CustomerName = SL.CustomerName,
                        TransactionDate = Convert.ToDateTime(SL.LedgerDate),
                        Reference = SL.Reference,
                        Remarks = SL.Description,
                        Debit = Convert.ToDouble(SL.Debit),
                        Credit = Convert.ToDouble(SL.Credit),
                        FromDate = Convert.ToDateTime(SL.FromDate),
                        ToDate = Convert.ToDateTime(SL.ToDate),
                        Source = SL.Source,
                    });
                }
                return CustomerLedgerL;
            }
        }

        public static DataTable BuildDataTable<T>(IList<T> lst)
        {
            //create DataTable Structure
            DataTable tbl = CreateTable<T>();
            Type entType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);
            //get the list item and add into the list
            foreach (T item in lst)
            {
                DataRow row = tbl.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    if (prop.PropertyType.Name != "Nullable`1")
                        row[prop.Name] = prop.GetValue(item);
                    else
                        row[prop.Name] = prop.GetValue(item);
                }
                tbl.Rows.Add(row);
            }
            return tbl;
        }

        private static DataTable CreateTable<T>()
        {
            //T –> ClassName
            Type entType = typeof(T);
            //set the datatable name as class name
            DataTable tbl = new DataTable(entType.Name);
            //get the property list
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);
            foreach (PropertyDescriptor prop in properties)
            {
                if (prop.PropertyType.Name != "Nullable`1")
                    tbl.Columns.Add(prop.Name, prop.PropertyType);
                else
                    tbl.Columns.Add(prop.Name, typeof(string));
                //add property as column

            }
            return tbl;
        }

    }
}