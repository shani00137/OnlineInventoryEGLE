using OnlineInventory.DbContext;
using OnlineInventory.Models;
using OnlineInventory.Utilites.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineInventory.DAL
{
    public class UserAccount
    {
        public static bool UpdatePassword(UserLoginModel obj)
        {
            bool Response = false;
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {
                String CurrentPassword= Encryption.Encrypt(obj.Password);
                var CheckPasswordMatach = dbContaxt.UserLoginTbls.Where(x => x.LoginId == obj.LoginId && x.Password == CurrentPassword).FirstOrDefault();
                if (CheckPasswordMatach!=null)
                {
                    CheckPasswordMatach.Password= Encryption.Encrypt(obj.NewPassword);
                    dbContaxt.SaveChanges();
                    Response = true;
                }
                else
                {
                    Response = false;
                }
            }
            return Response;
        }
    }
}