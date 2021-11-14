using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    
    public class ShopingCartModel
    {
        public List<Item> Items { get; set; }
        public List<ShopingCartItem> ShopingCart { get; set; }

        public ShopingCartModel()
        {
            Items = new List<Item>();
            ShopingCart = new List<ShopingCartItem>();
        }


    }
}