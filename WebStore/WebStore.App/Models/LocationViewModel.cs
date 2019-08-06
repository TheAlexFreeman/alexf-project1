using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

using WebStore.BLL;

namespace WebStore.App.Models
{
    public class LocationViewModel
    {
        public LocationViewModel(Location location)
        {
            Id = location.Id;
            Name = location.Name;
            Prices = new Dictionary<string, double>(location.Products.Select(p => new KeyValuePair<string, double>(p.Name, p.Price)));
            Inv = new Dictionary<string, int>(location.ItemsInStock.Select(i => new KeyValuePair<string, int>(i.Name, location.Count(i))));
        }

        public LocationViewModel() { }

        [Display(Name = "ID")]
        public int Id { get; set; }
        [Display(Name = "Store Name")]
        public string Name { get; set; }
        public readonly Dictionary<string, int> Inv;
        public readonly Dictionary<string, double> Prices;

        public int Count(string item) { return Inv[item]; }
       
        public Location AsLocation()
        {
            var location = new Location(Name, id: Id);
            foreach (string productName in Prices.Keys)
            {
                location.Products.Add(new Product(productName, Prices[productName]));
            }
            return location;
        }
    }
}
