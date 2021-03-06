using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using WebApplication2.Models;
using EASendMail;


namespace WebApplication2.Controllers
{
    [RequireHttps]
    public class LoginController : Controller
    {
        private const string USER_LOGIN = "LoginUserNameView";
        private const string PASSWORD_LOGIN = "LoginPasswordView";
        private const string HOME_VIEW = "/Views/Home/Index.cshtml";
        
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddUserView()
        {
            Logger.WriteActionLog("GetNewUserView", Session["user"] as int?, Session.SessionID);
            return View("AddUserView");
        }

        public ActionResult LogInPasswordView()
        {
            Logger.WriteActionLog("GetLogInPasswordView", Session["user"] as int?, Session.SessionID);
            return View(PASSWORD_LOGIN);
        }

        public ActionResult LogInUserNameView()
        {
            Logger.WriteActionLog("GetLogInUserNameView", Session["user"] as int?, Session.SessionID);
            ViewData["incorrectCred"] = false;
            if (Session["user"] != null)
            {
                if (UserModel.GetUser((int)Session["user"]).LogedIn)
                {
                    ViewData["username"] = UserModel.GetUser((int)Session["user"]).UserName;
                    ViewData["email"] = UserModel.GetUser((int)Session["user"]).email;
                    return View("LogoutView");
                }
                else
                {
                    Session.Clear();
                    return View(USER_LOGIN);
                }
            }
            else
            {
                return View(USER_LOGIN);
            }

            
        }


        //When the username is submited
        [HttpPost]
        public ActionResult LoginUsername(FormCollection collection)
        {
            
            Session.Clear();
            List<UserModel> users = UserModel.GetUsers();
            string username = "";
            foreach(UserModel user in users)
            {
                if(user.UserName == collection["username"] && user.PasswordHash == HashPassword(collection["password"]))
                {
                    username = user.UserName;
                    int uId = user.id;
                    string authCode = GenerateAuthCode();
                    string authHash = HashPassword(authCode);
                    SendAuthCode(uId, authCode);
                    user.PasswordHash = authHash;
                    UserModel.ChangeUserAuthCode(uId, authHash);
                    Session["user"] = user.id;
                    break;
                }    
            }
            if(username == "") 
            {
                ViewData["incorrectCred"] = true;
                return View(USER_LOGIN);
            }
            Logger.WriteActionLog("PostLoginUsername", Session["user"] as int?, Session.SessionID);
            return View(PASSWORD_LOGIN);
        }

        //when the password is submited
        [HttpPost]
        public ActionResult LoginPassword(FormCollection collection)
        {
            Logger.WriteActionLog("PostLoginPassword", Session["user"] as int?, Session.SessionID);
            if (UserModel.GetUser(Session["user"] as int?).AuthCodeHash == HashPassword(collection["password"]))
            {
                UserModel.LogUserIn((int)Session["user"]);
                Session.Timeout = 15;
                return View(HOME_VIEW);
            }
            else
            {
                Session.Abandon();
                Session.Clear();
                return View(USER_LOGIN);
            }
           
        }


        [HttpPost]
        public ActionResult AddUser(FormCollection collection)
        {
            Logger.WriteActionLog("PostAddUser", Session["user"] as int?, Session.SessionID);
            try
            {
                bool vaidInput = true;
                if (!CheakEmailFormat(collection["email"]))
                {
                    vaidInput = false;
                    ViewData["InvalidEmail"] = true;
                }
                if (!CheckIfUserNameIsUsed(collection["name"]))
                {
                    vaidInput = false;
                    ViewData["InvalidUserName"] = true;
                }
                if (!CheckForXSS(collection["email"]))
                {
                    vaidInput = false;
                    ViewData["InvalidEmail"] = true;
                }
                if (!CheckForXSS(collection["name"]))
                {
                    vaidInput = false;
                    ViewData["InvalidUserName"] = true;
                }
                if (vaidInput)
                {
                    UserModel.AddUser(collection["name"], collection["email"], HashPassword(collection["password"]));
                    Session.Clear();
                    return View(USER_LOGIN);
                }
                else
                {
                    Session.Clear();
                    return View("AddUserView");
                }
            }
            catch(Exception e)
            {
                return View("AddUserView");
            }

            
            
            
        }

        public ActionResult LogOut()
        {
            Logger.WriteActionLog("Logout", Session["user"] as int?, Session.SessionID);
            UserModel.LogUserOut((int)Session["user"]);
            Session.Clear();
            Session.Abandon();
            return View(USER_LOGIN);
        }

        private string HashPassword(string password)
        {
            var sha1 = SHA1.Create();
            var inputBytes = Encoding.ASCII.GetBytes(password);
            var hash = sha1.ComputeHash(inputBytes);
            var stringBuilder = new StringBuilder();
            
            for (var i = 0; i < hash.Length; i++)
            {
                stringBuilder.Append(hash[i].ToString("X2"));
            }
            string hashPass = stringBuilder.ToString();
            return hashPass;
        }

        private string GenerateAuthCode()
        {
            string password = "";
            Random random = new Random();
            for(int i = 0; i < 15; i++)
            {
                int numOrLeter = random.Next(1, 10);
                char randomChar;
                if(numOrLeter <= 5) //numbers
                {
                    randomChar = Convert.ToChar(random.Next(48,57));
                }
                else //letters
                {
                    if(random.Next(1,10) <= 5) //upper case
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
        private void SendAuthCode(int id, string password)
        {
            try
            {
                SmtpMail oMail = new SmtpMail("TryIt");

                oMail.From = "UWPlattBakerySite@gmail.com";
                oMail.To = UserModel.GetUser(id).email;

               
                oMail.Subject = "UWPlatt Bakery Authentication Code";
                oMail.TextBody = password;
                SmtpServer oServer = new SmtpServer("smtp.gmail.com");
                
                oServer.User = "UWPlattBakerySite@gmail.com";
                oServer.Password = "uwplatt7898!!";
                oServer.Port = 465;
                oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

                SmtpClient oSmtp = new SmtpClient();
                oSmtp.SendMail(oServer, oMail);

                
            }
            catch (Exception ep)
            {
                Console.WriteLine(ep);
            }
        }

        private bool CheakEmailFormat(string email)
        {
            if (email.Contains("@") && email.Contains("."))
            {
                string[] parts = email.Split('@');
                if (parts.Length == 2)
                {
                    string[] subParts = parts[1].Split('.');
                    if (subParts.Length == 2)
                    {
                        if (subParts[1].Length == 3)
                        {
                            return true;
                        }
                    }

                }
            }
            return false;
        }

        private bool CheckIfUserNameIsUsed(string username)
        {
            List<UserModel> models = UserModel.GetUsers();
            foreach(UserModel model in models)
            {
                if(model.UserName == username)
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckForXSS(string value)
        {
            string scritpTag = "<SCRIPT>";
            string divTag = "<DIV";
            if(value.Contains(scritpTag) || value.Contains(scritpTag.ToLower()) || value.Contains(divTag) || value.Contains(divTag.ToLower()))
            {
                return false;
            }
            return true;
        }

        
       
    }
}