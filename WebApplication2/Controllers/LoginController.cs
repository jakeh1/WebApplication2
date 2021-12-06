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

namespace WebApplication2.Controllers
{
    public class LoginController : Controller
    {
        private const string PATH = "Data//XML//UserData.xml";
        
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        //When the username is submited
        [HttpPost]
        public ActionResult LoginUsername(FormCollection collection)
        {
            List<UserModel> users = GetUsers();
            string username = "";
            
            foreach(UserModel user in users)
            {
                if(user.UserName == collection["username"])
                {
                    username = user.UserName;
                    int uId = user.id;
                    string tempPass = GeneratePassword();
                    string passHash = HashPassword(tempPass);
                    SendPassword(uId, tempPass);
                    user.PasswordHash = passHash;
                    Session["user"] = user.id;
                    break;
                }    
            }
            if(username == "") 
            {
                //Return error message
            }
            return View();
        }

        //when the password is submited
        [HttpPost]
        public ActionResult LoginPassword(FormCollection collection)
        {
            string viewName = "Login";
            if(GetUser(Session["user"] as int?).PasswordHash == HashPassword(collection["password"]))
            {
                Session["acess"] = true;
                
            }
            else
            {
                Session.Clear();
                //error messgae send back to login page.
            }
        }


        [HttpPost]
        public ActionResult AddUser(FormCollection collection)
        {
            //Add uesr to the xml file.
        }

        public ActionResult LogOut()
        {
            Session.Clear();
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
        private void SendPassword(int id, string password)
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
            List<UserModel> userModels = new List<UserModel>();
            XmlDocument xmlDocument = new XmlDocument();
            FileStream file = new FileStream(PATH, FileMode.Open);
            xmlDocument.Load(file);
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("user");
            foreach(XmlNode node in nodeList)
            {
                UserModel userModel = new UserModel();
                userModel.id = int.Parse(node.Attributes[0].Value);
                userModel.temporary = false;
                userModel.UserName = node.Attributes[1].Value;
                userModel.PasswordHash = node.Attributes[2].Value;
                userModel.email = node.Attributes[3].Value;
                userModels.Add(userModel);
            }
            file.Close();
            return userModels;
        }

        private UserModel GetUser(int? id)
        {
            if(id != null)
            {
                List<UserModel> userModels = GetUsers();
                foreach (UserModel user in userModels)
                {
                    if (user.id == id)
                    {
                        return user;
                    }
                }
            }
            return null;
        }
    }
}