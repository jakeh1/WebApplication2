using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [RequireHttps]
    public class HistoryController : Controller
    {
        private const int ID_INDEX = 0;
        private const int NAME_INDEX = 1;
        private const int PRICE_INDEX = 2;
        private const int AMOUNT_INDEX = 3;
        private string SHOPING_CART_PATH = Path.GetFullPath("Data//XML//ShopingCartData.xml");


        public ActionResult Index()
        {
            return View();
        }

        // GET: History
        public ActionResult HistoryView()
        {
            Logger.WriteActionLog("GetHistory", Session["user"] as int?, Session.SessionID);
            if(Session["user"] as int? != null)
            {
                UserModel userModel = UserModel.GetUser((int)Session["user"]);
                if (userModel.LogedIn && !userModel.temporary)
                {
                    List<ShopingCartModel> carts = GetShopingCarts(userModel.id);
                    ViewData["history"] = carts;
                    ViewData["userName"] = userModel.UserName;
                }
                else
                {
                    return View("/Views/Login/LogInUserNameView.cshtml");
                }
            }
            else
            {
                return View("/Views/Login/LogInUserNameView.cshtml");
            }
            return View();
        }

        private List<ShopingCartModel> GetShopingCarts(int userID)
        {
            List<ShopingCartModel> cartHistory = new List<ShopingCartModel>();
            XmlDocument xmlDocument = new XmlDocument();
            FileStream file = new FileStream(SHOPING_CART_PATH, FileMode.Open);
            xmlDocument.Load(file);
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("cart");
            foreach (XmlNode node in nodeList)
            {
                if ((int.Parse(node.Attributes[0].Value)) == userID && node.Attributes[1].Value == "F")
                {
                    ShopingCartModel shopingCartModel = new ShopingCartModel();
                    foreach (XmlNode xmlNode in node.ChildNodes)
                    {
                        ShopingCartItem item = new ShopingCartItem();
                        item.Id = int.Parse(xmlNode.Attributes[ID_INDEX].Value);
                        item.Name = xmlNode.Attributes[NAME_INDEX].Value;
                        item.Amount = int.Parse(xmlNode.Attributes[AMOUNT_INDEX].Value);
                        item.Price = double.Parse(xmlNode.Attributes[PRICE_INDEX].Value);
                        shopingCartModel.ShopingCart.Add(item);
                    }
                    cartHistory.Add(shopingCartModel);
                }
            }
            file.Close();
            return cartHistory;
        }
    }
}