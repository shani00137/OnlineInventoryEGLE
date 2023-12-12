using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineInventory.Models;
using OnlineInventory.DAL;
using OnlineInventory.DbContext;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace OnlineInventory.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        public ActionResult ReceiptVoucher()
        {
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {
                var fromDatabase = new SelectList(dbContaxt.CustomerTbls.ToList(), "CustomerId", "Name");
                ViewBag.CustomerData = fromDatabase;
            }
            ReceiptVoucherMD obj = new ReceiptVoucherMD();
            obj.VoucherNo = AccountsDB.GenerateVoucherNo();
            obj.VoucherDate = DateTime.Now.Date.ToString("MM/dd/yyyy");
            return View(obj);
        }
        [HttpPost]
        public JsonResult GetVoucherNo()
        {
            ReceiptVoucherMD obj = new ReceiptVoucherMD();
            obj.VoucherNo = AccountsDB.GenerateVoucherNo();
            obj.VoucherDate = DateTime.Now.Date.ToString("MM/dd/yyyy");
            return Json(obj);
        }
        [HttpPost]
        public ActionResult AddEditReceiptVoucher(ReceiptVoucherMD obj)
        {
            if (AccountsDB.AddEditReceipt(obj))
                return Json(new { success = true, responseText = "Saved successfuly!", Caseid = obj.VoucherID }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false, responseText = "Error While Saving data!", Caseid = obj.VoucherID }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadReceiptVouchers(int? CustomerID,String VoucherID)
        {
            var objVoucherList = AccountsDB.GetVouchersList(CustomerID, VoucherID);
            return Json(objVoucherList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadReceiptVoucherByID(int VoucherID)
        {
            var VouchersFeeEntry = AccountsDB.GetVouchersListByID(VoucherID);
            return Json(VouchersFeeEntry, JsonRequestBehavior.AllowGet);
        }


        #region CustomerLedger

        public ActionResult CustomerLedger()
        {
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {
                var fromDatabase = new SelectList(dbContaxt.CustomerTbls.ToList(), "CustomerId", "Name");
                ViewBag.CustomerData = fromDatabase;
            }
            JournalEntriesMD obj = new JournalEntriesMD();
            obj.FromDate = Convert.ToDateTime(DateTime.Now.Date.Month.ToString() + "/" + DateTime.Now.Date.Day.ToString() + "/" + DateTime.Now.Date.Year.ToString());
            obj.ToDate = Convert.ToDateTime(DateTime.Now.Date.Month.ToString() + "/" + DateTime.Now.Date.Day.ToString() + "/" + DateTime.Now.Date.Year.ToString());
            return View(obj);
        }

        public ActionResult ViewCustomerLedger(int CustomerId, DateTime FromDate, DateTime ToDate)
        {
            DataTable dt = new DataTable();
            var Data = AccountsDB.GetCustomerLedger(CustomerId, FromDate, ToDate);
            dt = AccountsDB.BuildDataTable(Data);
            //dt.WriteXml(@"E:\CustomerLedger.xml");

            ReportDocument rpt = new ReportDocument();
            rpt.Load(Server.MapPath("~/Reports/CustomerLedger.rpt"));
            rpt.SetDataSource(dt);
            Stream s = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            return File(s, "application/pdf");
        }

       
        public ActionResult ExportGeneralLedger(int CustomerID, DateTime FromDate, DateTime ToDate)
        {
            DataTable dt = ReportsDB.GetGeneralLedgerReport(CustomerID, FromDate, ToDate);
            ReportDocument rd = new ReportDocument();

            rd.Load(Server.MapPath("~/Reports/GeneralLedgerRPT.rpt"));

            rd.SetDataSource(dt);
            Stream s = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            return File(s, "application/pdf");
        }
        [HttpPost]
        public ActionResult ExportGeneralLedgerViewer(ReportMD value)
        {
            List < SaleInvoiceModel > list = ReportsDB.GetGeneralLedgerReportViwer(value.CustomerID, value.FromDate, value.ToDate);

           
            return Json(list, JsonRequestBehavior.AllowGet);
           
        }
        [HttpPost]
        public ActionResult ExportGeneralLedgerAllViewer(ReportMD value)
        {
            List<SaleInvoiceModel> list = ReportsDB.GetGeneralLedgerReportAllViwer(value.CustomerID);


            return Json(list, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}