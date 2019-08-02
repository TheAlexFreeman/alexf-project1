using System;
using System.Collections.Generic;

namespace WebStore.Data.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Order = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? DefaultStoreId { get; set; }

        public virtual Location DefaultStore { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
