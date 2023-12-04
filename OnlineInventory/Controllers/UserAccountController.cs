using OnlineInventory.DAL;
using OnlineInventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineInventory.Controllers
{
    public class UserAccountController : Controller
    {
        // GET: UserAccount
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdatePassword(UserLoginModel obj)
        {
            if (UserAccount.UpdatePassword(obj))
            {
                return Json(new { success = true, responseText = "Update Sucessfuly", response = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, responseText = "Something wrong Password not match please insert Correct Password..", response = false }, JsonRequestBehavior.AllowGet); ;
            }
               
           
        }
        [HttpGet]
        public ActionResult GetLoginUserId()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int LoginId = int.Parse(User.Identity.Name.Split('|')[1]); ;
                String UserName = User.Identity.Name.Split('|')[2];
                List<UserLoginModel> list = new List<UserLoginModel>();
                list.Add(new UserLoginModel
                {
                    UserName = UserName,
                    LoginId = LoginId
                });
                return Json(list, JsonRequestBehavior.AllowGet);
            }
                
            

        }
    }
}