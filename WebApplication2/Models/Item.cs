using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public Item Copy()
        {
            Item newItem = new Item();
            newItem.Id = this.Id;
            newItem.Name = this.Name;
            newItem.Price = this.Price;
            return newItem;
        }
    }
}
