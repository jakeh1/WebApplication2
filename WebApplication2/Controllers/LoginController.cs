using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class LoginController : Controller
    {
        private const string PATH = "";
        
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        //When the username is submited
        [HttpPost]
        public ActionResult LoginUsernameHistory(FromCollection collection)
        {
            List<UserModel> users = GetUsers();
            string username = "";
            int uId = -1;
            foreach(UserModel user in users)
            {
                if(user.UserName == collection["username"])
                {
                    username = user.UserName;
                    uId = user.id;
                    break;
                }
            }
            if(username == "") 
            {
                //Return error message
            }
            else
            {
                string tempPass = GeneratePassword();
                string passHash = HashPassword(tempPass);
                SendPassword(uId, tempPass);
                //set password hash to xml file
                //Return the password login view
            }
            
        }
        //when the password is submited
        [HttpPost]
        public ActionResult LoginPasswordHistory(FromCollection collection)
        {
            List<UserModel> users = GetUsers();
            foreach(UserModel user in users)
            {
                if(user.UserName == collection["username"] && user.PasswordHash == HashPassword(collection["password"]))
                {
                    //sends back the history view with the right stuff
                    break;
                }
                else
                {
                    //sends back error message with view.
                    //send back to start of login process with password being regentrated
                }
            }
        }
        private string HashPassword(string password)
        {
            var sha1 = SHA1.Create();
            var inputBytes = Encoding.ASCII.GetBytes(value);
            var hash = sha1.ComputeHash(inputBytes);
            var stringBuilder = new StringBuilder();
            
            for (var i = 0; i < hash.Length; i++)
            {
                stringBuilder.Append(hash[i].ToString("X2"));
            }
            string hashPass = stringBuilder.ToString();
            //Todo send hashed password to the file
            return hashPass;
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
        private void SendPassword(string username, string password)
        {
            //TODO open file and save to it
        }
        //Clears out the password and the password hash.
        private void ClearPassword(int uId)
        {
        }

        //Reads in the user data from the xml file
        private List<UserModel> GetUsers()
        {

        }
    }
}