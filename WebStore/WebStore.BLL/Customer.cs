using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebStore.BLL
{
    public class Customer
    {
        public Customer() { }
        public Customer(string firstName, string lastName, Location defaultStore = null, int id = 0)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DefaultStore = defaultStore;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        public Location DefaultStore { get; set; }


    }
}
