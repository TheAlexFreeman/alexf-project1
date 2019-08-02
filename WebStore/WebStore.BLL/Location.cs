using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebStore.BLL
{
    public class Location
    {
        public Location() {}
        public Location(string name, Inventory inv=null, IEnumerable<Product> products=null, int id=0)
        {
            Id = id;
            Name = name;
            Products = products?.ToList() ?? new List<Product>();
            Inv = inv ?? new Inventory();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        private readonly Inventory Inv;
        public readonly List<Product> Products;
        //private readonly List<Customer> Customers;
        //private readonly List<Order> Orders;

        public ISet<Item> ItemsInStock { get { return Inv.Items; } }
        
        public bool ProductAvailable(Product product, int quantity)
        {
            return Inv.ProductAvailable(product, quantity);
        }
        public bool ProductAvailable(Product product)
        {
            return Inv.ProductAvailable(product, 1);
        }
    }
}
