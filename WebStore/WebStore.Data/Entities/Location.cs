using System;
using System.Collections.Generic;

namespace WebStore.Data.Entities
{
    public partial class Location
    {
        public Location()
        {
            Customer = new HashSet<Customer>();
            InventoryItem = new HashSet<InventoryItem>();
            Order = new HashSet<Order>();
            ProductLocation = new HashSet<ProductLocation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Customer> Customer { get; set; }
        public virtual ICollection<InventoryItem> InventoryItem { get; set; }
        public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<ProductLocation> ProductLocation { get; set; }
    }
}
