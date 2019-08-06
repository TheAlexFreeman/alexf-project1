using Microsoft.AspNetCore.Mvc.Rendering;
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
            DefaultStore = customer.DefaultStore?.Name;
        }
        public CustomerViewModel() { }
        public CustomerViewModel(string firstName, string lastName, LocationViewModel defaultStore = null, int id = 0)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DefaultStore = defaultStore?.Name;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        public string DefaultStore { get; set; }
        public readonly List<SelectListItem> StoreOptions = new List<SelectListItem>
        {
            new SelectListItem{Value = "", Text = "(None)", Selected = true},
            new SelectListItem {Value = "Berkeley", Text = "Berkeley"},
            new SelectListItem {Value = "Arlington", Text = "Arlington"},
            new SelectListItem {Value = "Las Vegas", Text = "Las Vegas"}
        };


        public Customer AsCustomer
        {
            get
            {
                return new Customer(FirstName, LastName, DefaultStore == null ? null : new Location(DefaultStore), Id);
            }
        }
    }
}
