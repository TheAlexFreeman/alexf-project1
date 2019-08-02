using System;
using System.Collections.Generic;

namespace WebStore.Data.Entities
{
    public partial class Order
    {
        public Order()
        {
            ProductOrder = new HashSet<ProductOrder>();
        }

        public int Id { get; set; }
        public DateTime Time { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }

        public virtual Customer Buyer { get; set; }
        public virtual Location Seller { get; set; }
        public virtual ICollection<ProductOrder> ProductOrder { get; set; }
    }
}
