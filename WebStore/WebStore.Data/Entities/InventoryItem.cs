using System;
using System.Collections.Generic;

namespace WebStore.Data.Entities
{
    public partial class InventoryItem
    {
        public int LocationId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }

        public virtual Item Item { get; set; }
        public virtual Location Location { get; set; }
    }
}
