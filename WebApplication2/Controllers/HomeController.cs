using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Logger.WriteActionLog("GetIndex", Session["user"] as int?, Session.SessionID);
            return View();
        }

        public ActionResult About()
        {
            Logger.WriteActionLog("GetAbout", Session["user"] as int?, Session.SessionID);
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            Logger.WriteActionLog("GetContact", Session["user"] as int?, Session.SessionID);
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult HoursDirections()
        {
            Logger.WriteActionLog("GetHoursDirections", Session["user"] as int?, Session.SessionID);
            return View();
        }
        public ActionResult ItemsForSale()
        {
            Logger.WriteActionLog("ItemsForSale", Session["user"] as int?, Session.SessionID);
            return View();
        }
        public ActionResult News()
        {
            Logger.WriteActionLog("GetNews", Session["user"] as int?, Session.SessionID);
            return View();
        }
        public ActionResult PagesWithInfo()
        {
            Logger.WriteActionLog("GetPagesWithInfo", Session["user"] as int?, Session.SessionID);
            return View();
        }
        public ActionResult AJAXExample()
        {
            Logger.WriteActionLog("GetAJAXExample", Session["user"] as int?, Session.SessionID);
            return View();
        }
        //public ActionResult ShoppingCart()
        //{
        //    return View();
        //}

    }
}