using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }








        private void HashPassword(string password)
        {

        }

        private string GeneratePassword()
        {
            string password = "";
            Random random = new Random();
            for(int i = 0; i < 15; i++)
            {
                int numOrLeter = random.Next(0, 1);
                char randomChar;
                if(numOrLeter == 1) //numbers
                {
                    randomChar = Convert.ToChar(random.Next(48,57));
                }
                else //letters
                {
                    if(random.Next(0,1) == 0) //upper case
                    {
                        randomChar = Convert.ToChar(random.Next(65, 90));
                    }
                    else //lower case
                    {
                        randomChar = Convert.ToChar(random.Next(97, 122));
                    }
                    
                }
                password += randomChar;
            }
            return password;
        }
        private void SendPassword(string password)
        {

        }
    }
}