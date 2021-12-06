using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HistoryController : Controller
    {
        // GET: History
        public ActionResult Index()
        {
            if(Session["acess"] as bool? == true)
            {
                //add apropite data to the view bag
            }
            else
            {
                //send to login page.
            }
            return View();
        }

        private List<ShopingCartModel> GetShopingCarts(int userID)
        {
            //Xml query to find all shoping carts assocated with that user and put them in the list
        }
    }
}