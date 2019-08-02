using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebStore.BLL
{
    public class Product
    {
        public Product() { }
        public Product(string name, double price, int id = 0)
        {
            Id = id;
            Name = name;
            Price = price;
            Parts = new Dictionary<Item, int>();
        }
        public Product(string name, double price, Dictionary<Item, int> parts, int id = 0)
        {
            Id = id;
            Name = name;
            Price = price;
            Parts = parts;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        private readonly Dictionary<Item, int> Parts;
        public ISet<Item> Items { get { return new HashSet<Item>(Parts.Keys); } }
        public double SaleProfit { get { return Price - InventoryCost(); } }

        public int Count(Item item)
        {
            return Parts[item];
        }
        
        public double InventoryCost()
        {
            var total = 0.0;
            foreach(Item part in Items)
            {
                total += part.Cost * Parts[part];
            }
            return total;
        }
    }
}
