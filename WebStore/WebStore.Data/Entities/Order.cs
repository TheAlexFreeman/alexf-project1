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
        public DateTime Start { get; set; }
        public DateTime LastModified { get; set; }
        public bool IsOpen { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }

        public virtual Customer Buyer { get; set; }
        public virtual Location Seller { get; set; }
        public virtual ICollection<ProductOrder> ProductOrder { get; set; }
    }
}
