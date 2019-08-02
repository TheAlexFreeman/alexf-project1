using System;
using System.Collections.Generic;

namespace WebStore.Data.Entities
{
    public partial class ProductType
    {
        public ProductType()
        {
            InverseTypeOfNavigation = new HashSet<ProductType>();
            Product = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? TypeOf { get; set; }

        public virtual ProductType TypeOfNavigation { get; set; }
        public virtual ICollection<ProductType> InverseTypeOfNavigation { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}
