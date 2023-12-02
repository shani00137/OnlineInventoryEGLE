using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineInventory.Models;
using OnlineInventory.DAL;
using OnlineInventory.DbContext;
using System.Data.Entity;

namespace OnlineInventory.Controllers
{
    public class InventoryController : Controller
    {
        private static TimeZoneInfo Pakistan_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
        // GET: Inventory

        public ActionResult Items()
        {
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {
                var fromDatabase = new SelectList(dbContaxt.CustomerTbls.ToList(), "CustomerId", "Name");
                ViewBag.CustomerData = fromDatabase;
            }

            //List<ItemsMD> obj = InventoryDB.GetItemsList(0);
            return View();
        }

        [HttpPost]
        public ActionResult AddEditSessions(ItemsMD obj)
        {
            if (InventoryDB.AddEditItems(obj))
                return Json(new { success = true, responseText = "Saved successfuly!", Caseid = obj.ItemId }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false, responseText = "Error While Saving data!", Caseid = obj.ItemId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadGridItems(int CustomerId)
        {
            var VouchersFeeEntry = InventoryDB.GetItemsList(CustomerId);
            return Json(VouchersFeeEntry, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadItemsByCustomer(int customerId)
        {
            var VouchersFeeEntry = InventoryDB.GetItembyCustomers(customerId);
            return Json(VouchersFeeEntry, JsonRequestBehavior.AllowGet);
        }


        //Load Customes
        public ActionResult LoadGridCustomer(int customerId)
        {
            var VouchersFeeEntry = CustomerDb.GetCustomerList(customerId);
            return Json(VouchersFeeEntry, JsonRequestBehavior.AllowGet);
        }
        //saveCustomers
        [HttpPost]
        public ActionResult AddEditCustomers(CustomerMD obj)
        {
            if (CustomerDb.AddEditCustomer(obj))
                return Json(new { success = true, responseText = "Saved successfuly!", Caseid = obj.CustomerId }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false, responseText = "Error While Saving data!", Caseid = obj.CustomerId }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadGridItemByID(int ItemID)
        {
            var VouchersFeeEntry = InventoryDB.GetItemsListByID(ItemID);
            return Json(VouchersFeeEntry, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDashboardSaleChart()
        {
            DateTime Today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Pakistan_Standard_Time).Date;
            using (OnlineInvoiceSystemDBEntities db = new OnlineInvoiceSystemDBEntities())
            {
               
                DateTime StartDate = DateTime.Now.AddDays(-6);
                DateTime EndDate = Today;
                var query = (from c in db.InvoiceInfoTbls
                             join d in db.InvoiceInfoDetailTbls on c.InvoiceNo equals d.InvoiceNo
                             where c.InvoiceDate >= StartDate.Date && c.InvoiceDate <= EndDate.Date
                             select new
                             {
                                 c.InvoiceDate,
                                 d.Amount

                             }).OrderBy(x => x.InvoiceDate).OrderBy(x => x.InvoiceDate).ToList();
                var GroupByQuery = (from c in query
                                    group c by c.InvoiceDate into g
                                    select new { InvoiceDate = Convert.ToDateTime(g.Key).ToShortDateString(), Amount = g.Select(x => x.Amount).Sum() }).ToList();
                return Json(GroupByQuery, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult GetDashboardMonthlySaleChart()
        {
            DateTime Today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Pakistan_Standard_Time).Date;
            DateTime StartDate = DateTime.Now.AddDays(-30);
            DateTime EndDate = Today;
            using (OnlineInvoiceSystemDBEntities db = new OnlineInvoiceSystemDBEntities())
            {

                var query = (from c in db.InvoiceInfoTbls
                             join d in db.InvoiceInfoDetailTbls on c.InvoiceNo equals d.InvoiceNo
                             where c.InvoiceDate>=StartDate && c.InvoiceDate<=EndDate
                             select new
                             {
                                 c.InvoiceDate,
                                 d.Amount

                             }).OrderBy(x => x.InvoiceDate).OrderBy(x => x.InvoiceDate).ToList();
                var GroupByQuery = (from c in query
                                    group c by c.InvoiceDate into g
                                    select new { InvoiceDate = Convert.ToDateTime(g.Key).ToShortDateString(), Amount = g.Select(x => x.Amount).Sum() }).ToList();

               
                return Json(GroupByQuery, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult GetDashboardYearSaleChart()
        {
            DateTime now = DateTime.Now;
            DateTime Today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Pakistan_Standard_Time).Date;
            DateTime StartDate = Convert.ToDateTime(new DateTime(now.Year,1,1));
            DateTime EndDate = Today;
            using (OnlineInvoiceSystemDBEntities db = new OnlineInvoiceSystemDBEntities())
            {

                var query = (from c in db.InvoiceInfoTbls
                             join d in db.InvoiceInfoDetailTbls on c.InvoiceNo equals d.InvoiceNo
                             where c.InvoiceDate >= StartDate && c.InvoiceDate <= EndDate
                             select new
                             {
                                 c.InvoiceDate,
                                 d.Amount

                             }).OrderBy(x => x.InvoiceDate).OrderBy(x => x.InvoiceDate).ToList();
                var GroupByQuery = (from c in query
                                    group c by c.InvoiceDate.Month into g
                  
                                    select new { InvoiceDate =g.Key, Amount = g.Select(x => x.Amount).Sum() }).ToList();
                List<ChartModel> lst = new List<ChartModel>();
                foreach (var q in GroupByQuery)
                {
                    lst.Add(new ChartModel {
                    Amount=q.Amount,
                    InvoiceDate=Convert.ToDateTime(new DateTime(2000,q.InvoiceDate,1)).ToString("MMMM")
                    }); 
                }
                return Json(GroupByQuery, JsonRequestBehavior.AllowGet);
            }


        }
        List<int> MonthNames(DateTime date1, DateTime date2)
        {
            var monthList = new List<int>();

            while (date1 < date2)
            {
                monthList.Add(date1.Month);
                date1 = date1.AddMonths(1);
            }

            return monthList;
        }
        public ActionResult GetDashboardTopFigures()
        {
            double TotalSale = 0;
            double TodaySale = 0;
            int Customers = 0;
            double TotalDr = 0;
            DateTime TodayDate = DateTime.Now;
            List<DashaboardModel> list = new List<DashaboardModel>();
            using (OnlineInvoiceSystemDBEntities db = new OnlineInvoiceSystemDBEntities())
            {

                var TotalSaleQuery = (from c in db.InvoiceInfoTbls
                                      join d in db.InvoiceInfoDetailTbls on c.InvoiceNo equals d.InvoiceNo
                                      select new
                                      {
                                          c.InvoiceDate,
                                          d.Amount

                                      }).OrderBy(x => x.InvoiceDate).ToList();

                ///////////////////////////////TOTAL DEBTOR////////////////////////////
                var customers = db.CustomerTbls.Select(a => a.CustomerNo).ToList();
                double Dr = 0;
                double Cr = 0;
                double OpenBal = 0;
                double Bal = 0;
                foreach (var CustNo in customers)
                {
                    OpenBal = (double)db.CustomerTbls.Where(x => x.CustomerNo == CustNo).Select(x => x.OpeningBal).DefaultIfEmpty(0).Sum();
                    Dr = (double)db.JournalEntriesTbls.Where(x => x.Reference == CustNo).Select(x => x.Debit).DefaultIfEmpty(0).Sum();
                    Cr = (double)db.JournalEntriesTbls.Where(x => x.Reference == CustNo).Select(x => x.Credit).DefaultIfEmpty(0).Sum();
                    Bal = OpenBal + (Dr - Cr);
                    if (Bal > 0)
                        TotalDr =  TotalDr + Bal;
                }
                ///////////////////////////////////////////////////////////////////////

                TodaySale = (double)TotalSaleQuery.Where(x => x.InvoiceDate == TodayDate.Date).Select(x => x.Amount).Sum();
                TotalSale = TotalSaleQuery.Select(x => x.Amount).Sum();
                Customers = db.CustomerTbls.ToList().Count();
                list.Add(new DashaboardModel
                {
                    TotalSale = TotalSale,
                    TodaySale = TodaySale,
                    Customers = Customers,
                    TotalDebtor = TotalDr

                });

                return Json(list, JsonRequestBehavior.AllowGet);
            }


        }


    }
}