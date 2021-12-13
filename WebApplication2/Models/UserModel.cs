using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace WebApplication2.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string AuthCodeHash { get; set; }
        public int id { get; set; }
        public bool temporary { get; set; }
        public string email { get; set; }
        public bool LogedIn { get; set; }
        private const string PATH = "Data//XML//UserData.xml";


        public static List<UserModel> GetUsers()
        {
            List<UserModel> userModels = new List<UserModel>();
            XmlDocument xmlDocument = new XmlDocument();
            FileStream file = new FileStream(PATH, FileMode.Open);
            xmlDocument.Load(file);
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("user");
            foreach (XmlNode node in nodeList)
            {
                UserModel userModel = new UserModel();
                userModel.id = int.Parse(node.Attributes[0].Value);
                userModel.temporary = false;
                userModel.UserName = node.Attributes[1].Value;
                userModel.PasswordHash = node.Attributes[2].Value;
                userModel.email = node.Attributes[3].Value;
                userModel.LogedIn = node.Attributes[4].Value == "T";
                userModel.temporary = node.Attributes[5].Value == "T";
                userModel.AuthCodeHash = node.Attributes[6].Value;
                userModels.Add(userModel);
            }
            file.Close();
            return userModels;
        }

        public static UserModel GetUser(int? id)
        {
            if (id != null)
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

        public static void ChangeUserAuthCode(int uId, string authHash)
        {
            
            XmlDocument xmlDocument = new XmlDocument();
            FileStream file = new FileStream(PATH, FileMode.Open);
            xmlDocument.Load(file);
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("user");
            foreach (XmlNode node in nodeList)
            {
                if(int.Parse(node.Attributes[0].Value) == uId)
                {
                    node.Attributes[6].Value = authHash;
                }
            }
            file.Close();
            xmlDocument.Save(PATH);
        }

        public static void LogUserOut(int uId)
        {
            XmlDocument xmlDocument = new XmlDocument();
            FileStream file = new FileStream(PATH, FileMode.Open);
            xmlDocument.Load(file);
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("user");
            foreach (XmlNode node in nodeList)
            {
                if (int.Parse(node.Attributes[0].Value) == uId)
                {
                    node.Attributes[6].Value = "";
                    node.Attributes[4].Value = "F";
                }
            }
            file.Close();
            xmlDocument.Save(PATH);
        }

        public static void LogUserIn(int uId)
        {
            XmlDocument xmlDocument = new XmlDocument();
            FileStream file = new FileStream(PATH, FileMode.Open);
            xmlDocument.Load(file);
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("user");
            foreach (XmlNode node in nodeList)
            {
                if (int.Parse(node.Attributes[0].Value) == uId)
                {
                    node.Attributes[4].Value = "T";
                }
            }
            file.Close();
            xmlDocument.Save(PATH);
        }

        public static void AddUser(string name, string email, string passHash)
        {
            int maxId = 0;
            List<UserModel> userModels = GetUsers();
            foreach(UserModel user in userModels)
            {
                if(user.id > maxId)
                {
                    maxId = user.id;
                }
            }
            maxId++;
            XmlDocument xmlDocument = new XmlDocument();
            FileStream file = new FileStream(PATH, FileMode.Open);
            xmlDocument.Load(file);
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("users");
            foreach (XmlNode node in nodeList)
            {
                XmlElement newUser = xmlDocument.CreateElement("user");
                newUser.SetAttribute("id", maxId.ToString());
                newUser.SetAttribute("userName", name);
                newUser.SetAttribute("passHash", passHash);
                newUser.SetAttribute("email", email);
                newUser.SetAttribute("logedIn", "F");
                newUser.SetAttribute("temporary", "F");
                newUser.SetAttribute("authCodeHash", "");
                node.AppendChild(newUser);
            }
            file.Close();
            xmlDocument.Save(PATH);
        }
    }
}