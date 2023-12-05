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
        //Get Alluser list
        public static List<UserMD> GetAllUsers()
        {
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {
                var Query = (from c in dbContaxt.UserLoginTbls
                             select new UserMD {
                             UserName=c.UserName,
                             RoleName=c.RoleName,
                             IsActive=c.IsActive,
                             LoginId=c.LoginId
                             }).ToList();
                return Query;
            }
        }
        public static String SaveUser(UserMD value)
        {
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {
                var CheckUpdate= dbContaxt.UserLoginTbls.Where(x => x.LoginId==value.LoginId).FirstOrDefault();
                if (CheckUpdate == null)
                {
                    var CheckPresenace = dbContaxt.UserLoginTbls.Where(x => x.UserName.Contains(value.UserName)).FirstOrDefault();
                    if (CheckPresenace != null)
                    {
                        UserLoginTbl userLoginTbl = new UserLoginTbl();
                        userLoginTbl.UserName = value.UserName;
                        userLoginTbl.Password = Encryption.Encrypt("12345");
                        userLoginTbl.IsActive = true;
                        userLoginTbl.RoleName = value.RoleName;
                        dbContaxt.UserLoginTbls.Add(userLoginTbl);
                        dbContaxt.SaveChanges();
                        return "User Save Successfully";
                    }
                    else
                    {
                        return "User already present";
                    }
                }
                else {

                    //Update user
                    CheckUpdate.RoleName = value.RoleName;
                    CheckUpdate.IsActive = value.IsActive;                  
                    dbContaxt.SaveChanges();
                    return "User Updated Successfully";
                }
                
            }
        }
    }
}