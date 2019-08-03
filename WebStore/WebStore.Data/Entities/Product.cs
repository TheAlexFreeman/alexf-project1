using System;
using System.Collections.Generic;

namespace WebStore.Data.Entities
{
    public partial class Product
    {
        public Product()
        {
            ProductItem = new HashSet<ProductItem>();
            ProductLocation = new HashSet<ProductLocation>();
            ProductOrder = new HashSet<ProductOrder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<ProductItem> ProductItem { get; set; }
        public virtual ICollection<ProductLocation> ProductLocation { get; set; }
        public virtual ICollection<ProductOrder> ProductOrder { get; set; }
    }
}
