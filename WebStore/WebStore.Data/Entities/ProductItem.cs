using System;
using System.Collections.Generic;

namespace WebStore.Data.Entities
{
    public partial class ProductItem
    {
        public int ProductId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }

        public virtual Item Item { get; set; }
        public virtual Product Product { get; set; }
    }
}
