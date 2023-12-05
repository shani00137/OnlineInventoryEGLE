using OnlineInventory.DAL;
using OnlineInventory.DbContext;
using OnlineInventory.Models;
using OnlineInventory.Utilites.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
                        // User.Identity.Name.Split('|')[1]; = Query.LoginId;
                        //Session["UserName"] = Query.UserName;
                        Session["UserNo"] = Query.LoginId;
                        FormsAuthentication.SetAuthCookie(Query.RoleName + "|" + Query.LoginId+ "|" + Query.UserName, true);
                        TempData["RoleName"] = Query.RoleName;
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
           
        public ActionResult AddUser()
        {
            UserMD userMD = new UserMD();
            userMD.AllUserList= UserAccount.GetAllUsers();         
            return View(userMD);
        }
        [HttpPost]
        public ActionResult AddUser(UserMD value)
        {
            UserMD userMD = new UserMD();
            var response = UserAccount.SaveUser(value);
            ModelState.Clear();
            userMD.AllUserList = UserAccount.GetAllUsers();
            return View(userMD);
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
