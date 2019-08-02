using System;
using System.Collections.Generic;

namespace WebStore.Data.Entities
{
    public partial class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int StoreId { get; set; }

        public virtual Location Store { get; set; }
    }
}
