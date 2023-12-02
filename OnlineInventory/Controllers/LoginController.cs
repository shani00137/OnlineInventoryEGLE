using OnlineInventory.DbContext;
using OnlineInventory.Models;
using OnlineInventory.Utilites.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineInventory.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }


        // POST: Login/LoginNow
        [HttpPost]
        public ActionResult LoginNow(string UserName, string Password)
        {
            try
            {
              
                using (OnlineInvoiceSystemDBEntities db = new OnlineInvoiceSystemDBEntities())
                {
                    String EncryptPassword = Encryption.Encrypt(Password);
                    //String EncryptPassword = Password;
                    var Query = db.UserLoginTbls.Where(x => x.UserName == UserName && x.Password == EncryptPassword).FirstOrDefault();
                    if (Query != null)
                    {
                        Session["LoginId"] = Query.LoginId;
                        Session["UserName"] = Query.UserName;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["message"] = "Wrong User or Password";
                        return RedirectToAction("Index");
                    }
                }


            }
            catch(Exception ex)
            {

                return View();
            }
            
        }
           

        // GET: Login/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Login/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Login/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
