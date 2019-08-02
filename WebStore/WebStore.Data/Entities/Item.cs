using System;
using System.Collections.Generic;

namespace WebStore.Data.Entities
{
    public partial class Item
    {
        public Item()
        {
            InventoryItem = new HashSet<InventoryItem>();
            ProductItem = new HashSet<ProductItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }

        public virtual ICollection<InventoryItem> InventoryItem { get; set; }
        public virtual ICollection<ProductItem> ProductItem { get; set; }
    }
}
