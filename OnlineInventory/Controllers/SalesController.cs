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
        public ActionResult ViewInvoices(int? CustId, DateTime? FromDate, DateTime? ToDate, String InvoiceNo, int page = 1, int pageSize = 10, String UnitversityName = null)
        {
            InvoiceInfoMD obj = new InvoiceInfoMD();
            obj.PageSize = 20;
            DateTime dtfrom = DateTime.Now.Date;
            DateTime dtTo = DateTime.Now.Date;
            //List<InvoiceInfoMD> obj = SalesDb.GetAllInvoices(CustId, FromDate, ToDate, InvoiceNo);
            var response = SalesDb.GetAllInvoices(CustId, FromDate, ToDate, InvoiceNo);
            var CustomerList= CustomerDb.GetCustomerList(0);
            var totalItems = response.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var paginatedItems = response.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            obj.TotalPages = totalPages;
            obj.TotalItems = totalItems;
            obj.InvoiceList = paginatedItems;
            obj.CurrentPage = page;
            obj.CustomerId = CustId==null?0: (int)CustId;
            ViewBag.CutomerData = new SelectList(CustomerList, "CustomerId", "Name"); ;
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
        public ActionResult SaleInvoiceReturn(string InvoiceNo)
        {
            ViewBag.Inv = InvoiceNo;
            return View();
        }

        //public JsonResult GetAllInvoices(int? CustId, DateTime? FromDate, DateTime? ToDate,String InvoiceNo)
        //{
        //    var VouchersFeeEntry = SalesDb.GetAllInvoices(CustId, FromDate, ToDate, InvoiceNo);
        //    var jsonSettings = new JsonSerializerSettings();
        //    jsonSettings.DateFormatString = "dd/MM/yyyy";
        //    return Json(VouchersFeeEntry, JsonRequestBehavior.AllowGet);
        //}
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
            obj.CreatedBy = int.Parse(User.Identity.Name.Split('|')[1].ToString());
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
        public ActionResult GetReturnInvoiceByNo(string InvoiceNo)
        {
            var VouchersFeeEntry = SalesDb.LoadReturnInvoiceByNo(InvoiceNo);
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
            var Status = SalesDb.DeleteSaleInvoice(InvoiceNo);
            return Json(Status, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteReturnSaleInvoice(String InvoiceNo)
        {
            var Status = SalesDb.DeleteReturnSaleInvoice(InvoiceNo);
            return Json(Status, JsonRequestBehavior.AllowGet);
        }
        
    }
}