using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.BLL;

namespace WebStore.App.Models
{
    public class CustomerViewModel
    {
        public CustomerViewModel(Customer customer)
        {
            Id = customer.Id;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            DefaultStore = new LocationViewModel(customer.DefaultStore);
        }
        public CustomerViewModel() { }
        public CustomerViewModel(string firstName, string lastName, LocationViewModel defaultStore = null, int id = 0)
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
        public LocationViewModel DefaultStore { get; set; }


        public Customer AsCustomer
        {
            get
            {
                return new Customer(FirstName, LastName, DefaultStore.AsLocation, Id);
            }
        }
    }
}
