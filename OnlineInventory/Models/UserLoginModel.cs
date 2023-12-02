using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineInventory.Models
{
    public class UserLoginModel
    {
        public int LoginId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public bool IsActive { get; set; }
    }
}