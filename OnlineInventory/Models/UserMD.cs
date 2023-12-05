using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineInventory.Models
{
    public class UserMD
    {
        public int LoginId { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public bool IsActive { get; set; }
        public String RoleName { get; set; }
        public List<UserMD> AllUserList { get; set; }
    }
}