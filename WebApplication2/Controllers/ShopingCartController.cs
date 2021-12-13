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
    public class ShopingCartController : Controller
    {
        private const int BREAD = 0;
        private const int BANANA_BREAD = 1;
        private const int COOKIES = 2;
        private const int MUFFINS = 3;
        private const int DONUTS = 4;
        private const int ID_INDEX = 0;
        private const int NAME_INDEX = 1;
        private const int PRICE_INDEX = 2;
        private const int AMOUNT_INDEX = 3;

        private string ITEMS_PATH = Path.GetFullPath("Data//XML//ItemData.xml");
        private  string SHOPING_CART_PATH = Path.GetFullPath("Data//XML//ShopingCartData.xml");
        private ShopingCartModel shopingCartModel;

       

      

        public ActionResult ShoppingCart()
        {
            Logger.WriteActionLog("GetShoppingCart", Session["user"] as int?, Session.SessionID);
            if (Session["user"] == null)
            {
                return View("/Views/Login/LogInUserNameView.cshtml");
            }
            else if(!UserModel.GetUser((int)Session["user"]).LogedIn)
            {
                return View("/Views/Login/LogInUserNameView.cshtml");
            }
            else
            {
                shopingCartModel = new ShopingCartModel();
                ReadInItemData();
                ReadInShopingCartData();
                this.ViewData["cart"] = shopingCartModel.ShopingCart;

                return View();
            }
            
        }

        // GET: ShopingCart/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        [HttpPost]
        public ActionResult PlaceOrder(FormCollection collection)
        {
            Logger.WriteActionLog("PostPlaceOrder", Session["user"] as int?, Session.SessionID);
            string cridtCardNum = collection["creditCartNumber"];
            int userId = (int)Session["user"];
            XmlDocument xmlDocument = new XmlDocument();
            FileStream file = new FileStream(SHOPING_CART_PATH, FileMode.Open);
            xmlDocument.Load(file);
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("cart");
            foreach (XmlNode node in nodeList)
            {
                if (int.Parse(node.Attributes[0].Value) == userId && node.Attributes[1].Value == "T")
                {
                    node.Attributes[1].Value = "F";
                }
            }
            file.Close();
            xmlDocument.Save(SHOPING_CART_PATH);
            return View("OrderPlacedView");
        }

        // GET: ShopingCart/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShopingCart/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ShopingCart/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ShopingCart/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ShopingCart/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ShopingCart/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //Adds Bread
        public ActionResult AddActionBread()
        {
            AddAction(BREAD);
            return View("ShoppingCart");
        }
        //Adds Banana Bread
        public ActionResult AddActionBananaBread()
        {
            AddAction(BANANA_BREAD);
            return View("ShoppingCart");
        }
        //Adds Cookies
        public ActionResult AddActionCookies()
        {
            AddAction(COOKIES);
            return View("ShoppingCart");
        }
        //Adds Muffins
        public ActionResult AddActionMuffins()
        {
            AddAction(MUFFINS);
            return View("ShoppingCart");
        }
        //Adds donuts
        public ActionResult AddActionDonuts()
        {
            AddAction(DONUTS);
            return View("ShoppingCart");
        }

        //Adds Item of id to the xml
        private void AddAction(int id)
        {
            shopingCartModel = new ShopingCartModel();
            ReadInItemData();
            ReadInShopingCartData();
            AddOrChangeAmount(id);
            ReadInShopingCartData();
            this.ViewData["cart"] = shopingCartModel.ShopingCart;
        }


        //checks to see if an item needs to be added or amount incermented
        private void AddOrChangeAmount(int id)
        {
            int amount = this.CheckModelForItem(id);
            if (amount != 0)
            {
                amount++;
                this.ChangeAmountOfItemInShopingCart(id, amount);
            }
            else
            {
                this.AddItemToShopingCart(id);
            }
        }

        //Removes Bread
        public ActionResult RemoveActionBread()
        {
            RemoveAction(BREAD);
            return View("ShoppingCart");
        }
        //Removes Banana Bread
        public ActionResult RemoveActionBananaBread()
        {
            RemoveAction(BANANA_BREAD);
            return View("ShoppingCart");
        }
        //Removes Cookies
        public ActionResult RemoveActionCookies()
        {
            RemoveAction(COOKIES);
            return View("ShoppingCart");
        }
        //Removes Muffins
        public ActionResult RemoveActionMuffins()
        {
            RemoveAction(MUFFINS);
            return View("ShoppingCart");
        }
        //Removes Donuts
        public ActionResult RemoveActionDonuts()
        {
            RemoveAction(DONUTS);
            return View("ShoppingCart");
        }

        [HttpPost]
        public ActionResult UpdateCartData(FormCollection collection)
        {
            Logger.WriteActionLog("PostUpdateCartData", Session["user"] as int?, Session.SessionID);
            bool validInput = true;
            int[] ids = { BREAD, BANANA_BREAD, COOKIES, MUFFINS, DONUTS };
            string[] stringValues = { collection["breadText"], collection["bananaText"], collection["cookiesText"], collection["muffinText"], collection["donutText"] };
            int[] values = new int[5];
            for(int i = 0; i < 5; i++)
            {
                if (!int.TryParse(stringValues[i], out values[i]))
                {
                    validInput = false;
                    break;
                }
                else
                {
                    if(values[i] < 0)
                    {
                        validInput = false;
                        break;
                    }
                }
            }

            if (validInput)
            {
                shopingCartModel = new ShopingCartModel();
                ReadInItemData();
                ReadInShopingCartData();
                for (int i = 0; i < 5; i++)
                {
                    int amountInModel = this.CheckModelForItem(ids[i]);
                    if(amountInModel == 0)
                    {
                        if(values[i] != 0)
                        {
                            this.AddItemToShopingCart(ids[i]);
                            this.ChangeAmountOfItemInShopingCart(ids[i], values[i]);
                        }
                    }
                    else
                    {
                        if(values[i] == 0)
                        {
                            this.RemoveItemFromShopingCart(ids[i]);
                        }
                        else
                        {
                            this.ChangeAmountOfItemInShopingCart(ids[i], values[i]);
                        }
                    }
                }
                ReadInShopingCartData();
                this.ViewData["cart"] = shopingCartModel.ShopingCart;
                ViewData["invalidInput"] = false;
            }
            else
            {
                shopingCartModel = new ShopingCartModel();
                ReadInItemData();
                ReadInShopingCartData();
                this.ViewData["cart"] = shopingCartModel.ShopingCart;
                ViewData["invalidInput"] = true;
            }
            return View("ShoppingCart");
        }


        //Removes item of the given id from the model
        private void RemoveAction(int id)
        {
            shopingCartModel = new ShopingCartModel();
            ReadInItemData();
            ReadInShopingCartData();
            RemoveOrChangeAmount(id);
            ReadInShopingCartData();
            this.ViewData["cart"] = shopingCartModel.ShopingCart;
        }
        //Checks to see if the item needs to be removed or amount incermented
        private void RemoveOrChangeAmount(int id)
        {
            int amount = this.CheckModelForItem(id);
            if (amount > 1)
            {
                amount--;
                this.ChangeAmountOfItemInShopingCart(id, amount);
            }
            else
            {
                this.RemoveItemFromShopingCart(id);
            }
        }

        //Adds an Item to the shoping cart xml file(create)
        private void AddItemToShopingCart(int id)
        {
            Item data = null;
            foreach (Item item in this.shopingCartModel.Items)
            {
                if (item.Id == id)
                {
                    data = item;
                    break;
                }
            }
            if (data != null)
            {
                int userId = (int)Session["user"];
                XmlDocument xmlDocument = new XmlDocument();
                FileStream file = new FileStream(SHOPING_CART_PATH, FileMode.Open);
                xmlDocument.Load(file);
                XmlElement newItem = xmlDocument.CreateElement("item");
                newItem.SetAttribute("id", data.Id.ToString());
                newItem.SetAttribute("name", data.Name);
                newItem.SetAttribute("price", data.Price.ToString());
                newItem.SetAttribute("amount", "1");
                
                XmlNodeList carts = xmlDocument.GetElementsByTagName("cart");
                foreach(XmlNode node in carts)
                {
                    if (int.Parse(node.Attributes[0].Value) == userId && node.Attributes[1].Value == "T")
                    {
                        node.AppendChild(newItem);
                        break;
                    }
                }
                file.Close();
                xmlDocument.Save(SHOPING_CART_PATH);
            }

        }

        //Removes and item form the shoping cart xml file(delete)
        private void RemoveItemFromShopingCart(int Id)
        {

            Item data = null;
            foreach (Item item in this.shopingCartModel.ShopingCart)
            {
                if (item.Id == Id)
                {
                    data = item;
                    break;
                }
            }
            if (data != null)
            {
                int userId = (int)Session["user"];
                XmlDocument xmlDocument = new XmlDocument();
                FileStream file = new FileStream(SHOPING_CART_PATH, FileMode.Open);
                xmlDocument.Load(file);
                XmlNodeList nodeList = xmlDocument.GetElementsByTagName("cart");
                foreach(XmlNode node in nodeList)
                {
                    if(int.Parse(node.Attributes[0].Value) == userId && node.Attributes[1].Value == "T")
                    {
                        XmlNode childToRemove = null;
                        foreach(XmlNode child in node.ChildNodes)
                        {
                            if (int.Parse(child.Attributes[ID_INDEX].Value) == Id)
                            {
                                childToRemove = child;
                                break;
                            }
                        }
                        if(childToRemove != null)
                        {
                            node.RemoveChild(childToRemove);
                        }
                    }
                }
                file.Close();
                xmlDocument.Save(SHOPING_CART_PATH);
            }
        }

        //Changes the amount of an item in the shoping cart xml file(update)
        private void ChangeAmountOfItemInShopingCart(int itemId, int amount)
        {
            int userId = (int)Session["user"];
            XmlDocument xmlDocument = new XmlDocument();
            FileStream file = new FileStream(SHOPING_CART_PATH, FileMode.Open);
            xmlDocument.Load(file);
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("cart");
            foreach (XmlNode node in nodeList)
            {
                if (int.Parse(node.Attributes[0].Value) == userId && node.Attributes[1].Value == "T")
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Attributes[ID_INDEX].Value == itemId.ToString())
                        {
                            childNode.Attributes[AMOUNT_INDEX].Value = amount.ToString();
                            break;
                        }
                    }
                }
            }
            file.Close();
            xmlDocument.Save(SHOPING_CART_PATH);
        }

        //Reads in the item data from the item xml file(select)
        private void ReadInItemData()
        {
            XmlDocument xmlDocument = new XmlDocument();
            FileStream file = new FileStream(ITEMS_PATH, FileMode.Open);
            xmlDocument.Load(file);
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("items");
            foreach (XmlNode node in nodeList)
            {
                foreach (XmlNode xmlNode in node.ChildNodes)
                {
                    Item item = new Item();
                    XmlAttribute attribute = xmlNode.Attributes[0];
                    item.Id = int.Parse(attribute.Value);
                    foreach (XmlNode childNode in xmlNode.ChildNodes)
                    {
                        if (childNode.Name == "name")
                        {
                            item.Name = childNode.InnerText;
                        }
                        else
                        {
                            item.Price = double.Parse(childNode.InnerText);
                        }

                    }
                    this.shopingCartModel.Items.Add(item);

                }

            }
            file.Close();
        }

        //reads in the shoping cart data from the shoping cart xml file(select)
        private void ReadInShopingCartData()
        {
            bool makeNewCart = true;
            if(Session["user"] == null)
            {
                return;
            }
            int userId = (int)Session["user"];
            shopingCartModel.ShopingCart = new List<ShopingCartItem>();
            XmlDocument xmlDocument = new XmlDocument();
            FileStream file = new FileStream(SHOPING_CART_PATH, FileMode.Open);
            xmlDocument.Load(file);
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("cart");
            foreach (XmlNode node in nodeList)
            {
                if((int.Parse(node.Attributes[0].Value)) == userId && node.Attributes[1].Value == "T")
                {
                    makeNewCart = false;
                    foreach (XmlNode xmlNode in node.ChildNodes)
                    { 
                        ShopingCartItem item = new ShopingCartItem();
                        item.Id = int.Parse(xmlNode.Attributes[ID_INDEX].Value);
                        item.Name = xmlNode.Attributes[NAME_INDEX].Value;
                        item.Amount = int.Parse(xmlNode.Attributes[AMOUNT_INDEX].Value);
                        item.Price = double.Parse(xmlNode.Attributes[PRICE_INDEX].Value);
                        shopingCartModel.ShopingCart.Add(item);
                    }
                }
            }
            file.Close();
            if (makeNewCart)
            {
                MakeNewCart();
            }
        }

        private void MakeNewCart()
        {
            XmlDocument xmlDocument = new XmlDocument();
            FileStream file = new FileStream(SHOPING_CART_PATH, FileMode.Open);
            xmlDocument.Load(file);
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("carts");
            foreach(XmlNode node in nodeList)
            {
                XmlElement newCart = xmlDocument.CreateElement("cart");
                newCart.SetAttribute("userId", Session["user"].ToString());
                newCart.SetAttribute("current", "T");
                node.AppendChild(newCart);
            }
            file.Close();
            xmlDocument.Save(SHOPING_CART_PATH);
        }

        //looks for the amount of an item in the model
        private int CheckModelForItem(int id)
        {
            int amount = 0;
            foreach(ShopingCartItem item in this.shopingCartModel.ShopingCart)
            {
                if(item.Id == id)
                {
                    amount = item.Amount;
                }
            }
            return amount;
        }

        

    }
}
