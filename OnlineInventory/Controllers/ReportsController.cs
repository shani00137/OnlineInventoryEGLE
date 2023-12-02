using CrystalDecisions.CrystalReports.Engine;
using OnlineInventory.DAL;
using OnlineInventory.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineInventory.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult ExportInvoiceReport(String InvoiceNo)
        {
            DataTable dt = ReportsDB.GetInvoiceReport(InvoiceNo);
            ReportDocument rd = new ReportDocument();

            rd.Load(Server.MapPath("~/Reports/InvoiceReportRPT.rpt"));

            rd.SetDataSource(dt);
            Stream s = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            return File(s, "application/pdf");
        }

        public ActionResult ExportItemReportsByCustomer(int CustomerId)
        {
            DataTable dt = ReportsDB.GetItemReportByCustomer(CustomerId);
            ReportDocument rd = new ReportDocument();

            rd.Load(Server.MapPath("~/Reports/CustomerItemsReport.rpt"));

            rd.SetDataSource(dt);
            Stream s = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            return File(s, "application/pdf");
        }

    }
}