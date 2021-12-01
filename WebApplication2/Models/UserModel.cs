using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public int id { get; set; }
        
    }
}