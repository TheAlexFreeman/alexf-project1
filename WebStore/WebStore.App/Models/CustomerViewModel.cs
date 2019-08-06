using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using WebStore.BLL;
using WebStore.BLL.Interfaces;

namespace WebStore.App.Models
{
    public class CustomerViewModel
    {
        //public static ICustomerRepository _customerRepo;
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
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Display(Name= "First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        [Display(Name = "Default Store")]
        public string DefaultStore { get; set; }
        public readonly List<SelectListItem> StoreOptions = new List<SelectListItem>
        {
            new SelectListItem{Value = "", Text = "(None)", Selected = true},
            new SelectListItem {Value = "Berkeley", Text = "Berkeley"},
            new SelectListItem {Value = "Arlington", Text = "Arlington"},
            new SelectListItem {Value = "Las Vegas", Text = "Las Vegas"}
        };
        public List<SelectListItem> CustomerOptions = new List<SelectListItem>
        {
            new SelectListItem{Value = "0", Text = "(None)"}
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
