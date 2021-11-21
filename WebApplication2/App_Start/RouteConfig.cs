using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ShopingCartRemoveBread",
                url: "ShopingCartController/{action}",
                defaults: new { controller = "ShopingCartController", action = "RemoveActionBread"}
            );
            routes.MapRoute(
               name: "ShopingCartBread",
               url: "{controller}/{action}",
               defaults: new { controller = "ShopingCartController", action = "AddActionBread"}
           );
            routes.MapRoute(
               name: "ShopingCartBananaBread",
               url: "{controller}/{action}",
               defaults: new { controller = "ShopingCartController", action = "AddActionBananaBread" }
           );
            routes.MapRoute(
               name: "ShopingCartRemoveBananaBread",
               url: "ShopingCartController/{action}",
               defaults: new { controller = "ShopingCartController", action = "RemoveActionBananaBread" }
           );
            routes.MapRoute(
               name: "ShopingCartCookies",
               url: "{controller}/{action}",
               defaults: new { controller = "ShopingCartController", action = "AddActionCookies" }
           );
            routes.MapRoute(
               name: "ShopingCartRemoveCookies",
               url: "ShopingCartController/{action}",
               defaults: new { controller = "ShopingCartController", action = "RemoveActionCookies" }
           );
            routes.MapRoute(
               name: "ShopingCartMuffins",
               url: "{controller}/{action}",
               defaults: new { controller = "ShopingCartController", action = "AddActionMuffins" }
           );
            routes.MapRoute(
               name: "ShopingCartRemoveMuffins",
               url: "ShopingCartController/{action}",
               defaults: new { controller = "ShopingCartController", action = "RemoveActionMuffins" }
           );
            routes.MapRoute(
               name: "ShopingCartDonuts",
               url: "{controller}/{action}",
               defaults: new { controller = "ShopingCartController", action = "AddActionDonuts" }
           );
            routes.MapRoute(
               name: "ShopingCartRemoveDonuts",
               url: "ShopingCartController/{action}",
               defaults: new { controller = "ShopingCartController", action = "RemoveActionDonuts" }
           );
            routes.MapRoute(
              name: "ShopingCartUpdateCartData",
              url: "ShopingCartController/{action}",
              defaults: new { controller = "ShopingCartController", action = "UpdateCartData" }
          );
        }
    }
}
