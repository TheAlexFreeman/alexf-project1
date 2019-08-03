using System;
using System.Collections.Generic;

namespace WebStore.Data.Entities
{
    public partial class ProductLocation
    {
        public int ProductId { get; set; }
        public int LocationId { get; set; }

        public virtual Location Location { get; set; }
        public virtual Product Product { get; set; }
    }
}
