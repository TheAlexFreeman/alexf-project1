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
            Parts = new Inventory();
        }
        public Product(string name, double price, Dictionary<Item, int> parts, int id = 0)
        {
            Id = id;
            Name = name;
            Price = price;
            Parts = new Inventory(parts);
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        private readonly Inventory Parts;
        public ISet<Item> Items { get { return Parts.Items; } }
        public double InventoryCost { get { return Parts.TotalCost; } }
        public double SaleProfit { get { return Price - InventoryCost; } }

        public int Count(Item item)
        {
            return Parts.Count(item);
        }
    }
}
