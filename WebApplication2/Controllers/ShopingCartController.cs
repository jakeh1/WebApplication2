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

        private string ITEMS_PATH = Path.GetFullPath("Data//XML//ItemData.xml");
        private  string SHOPING_CART_PATH = Path.GetFullPath("Data//XML//ShopingCartData.xml");
        private ShopingCartModel shopingCartModel;

       

      

        public ActionResult ShoppingCart()
        {
            shopingCartModel = new ShopingCartModel();
            ReadInItemData();
            ReadInShopingCartData();
            this.ViewData["cart"] = shopingCartModel.ShopingCart;

            return View();
        }

        // GET: ShopingCart/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
        public ActionResult UpdateCartData(int id, FormCollection collection)
        {
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
            }
            else
            {
                //Case where input was invalid somehow modify soping cart to produce error message return invalid input view.
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
                XmlDocument xmlDocument = new XmlDocument();
                FileStream file = new FileStream(SHOPING_CART_PATH, FileMode.Open);
                xmlDocument.Load(file);
                XmlElement newItem = xmlDocument.CreateElement("item");
                XmlElement newName = xmlDocument.CreateElement("name");
                XmlElement newPrice = xmlDocument.CreateElement("price");
                XmlElement newAmount = xmlDocument.CreateElement("amount");
                newItem.SetAttribute("id", data.Id.ToString());
                newName.InnerText = data.Name;
                newAmount.InnerText = "1";
                newPrice.InnerText = data.Price.ToString();
                newItem.AppendChild(newName);
                newItem.AppendChild(newPrice);
                newItem.AppendChild(newAmount);
                XmlElement cart = (XmlElement)xmlDocument.GetElementsByTagName("cart")[0];
                cart.AppendChild(newItem);
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
                XmlDocument xmlDocument = new XmlDocument();
                FileStream file = new FileStream(SHOPING_CART_PATH, FileMode.Open);
                xmlDocument.Load(file);
                XmlNodeList cart = xmlDocument.GetElementsByTagName("item");
                XmlElement cart2 = (XmlElement)xmlDocument.GetElementsByTagName("cart")[0];
                for (int i = 0; i < cart.Count; i++)
                {
                    XmlElement element = (XmlElement)cart[i];
                    if (element.GetAttribute("id") == Id.ToString())
                    {
                        cart2.RemoveChild(element);
                        break;
                    }
                }
                file.Close();
                xmlDocument.Save(SHOPING_CART_PATH);
            }
        }

        //Changes the amount of an item in the shoping cart xml file(update)
        private void ChangeAmountOfItemInShopingCart(int itemId, int amount)
        {
            XmlDocument xmlDocument = new XmlDocument();
            FileStream file = new FileStream(SHOPING_CART_PATH, FileMode.Open);
            xmlDocument.Load(file);
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("cart");
            foreach (XmlNode node in nodeList)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    if (childNode.Attributes[0].Value == itemId.ToString())
                    {
                        foreach (XmlNode xmlNode in childNode.ChildNodes)
                        {
                            if (xmlNode.Name == "amount")
                            {
                                xmlNode.InnerText = amount.ToString();
                            }
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
            this.shopingCartModel.ShopingCart = new List<ShopingCartItem>();
            XmlDocument xmlDocument = new XmlDocument();
            FileStream file = new FileStream(SHOPING_CART_PATH, FileMode.Open);
            xmlDocument.Load(file);
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("cart");
            foreach (XmlNode node in nodeList)
            {
                foreach (XmlNode xmlNode in node.ChildNodes)
                {
                    ShopingCartItem item = new ShopingCartItem();
                    XmlAttribute attribute = xmlNode.Attributes[0];
                    item.Id = int.Parse(attribute.Value);
                    foreach (XmlNode childNode in xmlNode.ChildNodes)
                    {
                        if (childNode.Name == "name")
                        {
                            item.Name = childNode.InnerText;
                        }
                        else if (childNode.Name == "price")
                        {
                            item.Price = double.Parse(childNode.InnerText);
                        }
                        else
                        {
                            item.Amount = int.Parse(childNode.InnerText);
                        }
                    }
                    this.shopingCartModel.ShopingCart.Add(item);

                }

            }
            file.Close();
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
