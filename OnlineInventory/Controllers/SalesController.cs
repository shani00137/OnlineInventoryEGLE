using Newtonsoft.Json;
using OnlineInventory.DAL;
using OnlineInventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace OnlineInventory.Controllers
{
    public class SalesController : Controller
    {
        // GET: Sales
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewInvoices()
        {
            DateTime dtfrom = DateTime.Now.Date;
            DateTime dtTo = DateTime.Now.Date;
            List<InvoiceInfoMD> obj = SalesDb.GetAllInvoices(0, dtfrom, dtTo);
            return View(obj);
        }
        public ActionResult ViewReturnInvoice()
        {
            List<InvoiceInfoMD> obj = SalesDb.GetAllReturnInvoice();
            return View(obj);
        }

        public ActionResult Customers()
        {
            List<CustomerMD> obj = CustomerDb.GetCustomerList(0); ;
            return View(obj);
         
        }
        public ActionResult SaleInvoice(string InvoiceNo)
        {
            ViewBag.Inv = InvoiceNo;
            return View();
        }
        public ActionResult SaleInvoiceReturn()
        {
            return View();
        }

        public JsonResult GetAllInvoices(int? CustId, DateTime? FromDate, DateTime? ToDate,String InvoiceNo)
        {
            var VouchersFeeEntry = SalesDb.GetAllInvoices(CustId, FromDate, ToDate);
            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = "dd/MM/yyyy";
            return Json(VouchersFeeEntry, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllReturnInvoices()
        {
            var VouchersFeeEntry = SalesDb.GetAllReturnInvoice();
           
            return Json(VouchersFeeEntry, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetReturnInvoiceDetails(String ReturnInvoiceNo)
        {
            var VouchersFeeEntry = SalesDb.GetReturnInvoiceDetails(ReturnInvoiceNo);
       
            return Json(VouchersFeeEntry, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveSaleInvoice(SaleInvoiceModel obj)
        {
            var Response = SalesDb.AddEditSaleInvoice(obj);
            if (Response!="")
                return Json(new { success = true, responseText = "Saved successfuly!", Caseid = Response }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false, responseText = "Error While Saving data!", Caseid = obj.InvoiceNo }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveReturnSaleInvoice(SaleInvoiceModel obj)
        {
            var Response = SalesDb.AddEditReturnSaleInvoice(obj);
            if (Response != "")
                return Json(new { success = true, responseText = "Saved successfuly!", Caseid = Response }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false, responseText = "Error While Saving data!", Caseid = obj.InvoiceNo }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetInvoiceByNo(string InvoiceNo)
        {
            var VouchersFeeEntry = SalesDb.LoadInvoiceByNo(InvoiceNo);
            return Json(VouchersFeeEntry, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerBalances()
        {
            //List<CustomerMD> obj = CustomerDb.GetCustomerList(0); ;
            return View();

        }
        public ActionResult GetCustomerBalances()
        {
            var CustomersBalances = SalesDb.LoadCustomerBalances();
            return Json(CustomersBalances, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteSaleInovice(String InvoiceNo)
        {
            bool Status = SalesDb.DeleteSaleInvoice(InvoiceNo);
            return Json(Status, JsonRequestBehavior.AllowGet);
        }

    }
}