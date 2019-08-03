using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.BLL;

namespace WebStore.App.Models
{
    public class ProductViewModel
    {
        public ProductViewModel() { }
        public ProductViewModel(string name, double price, int id = 0)
        {
            Id = id;
            Name = name;
            Price = price;
            Parts = new InventoryViewModel();
        }
        public ProductViewModel(string name, double price, Dictionary<ItemViewModel, int> parts, int id = 0)
        {
            Id = id;
            Name = name;
            Price = price;
            Parts = new InventoryViewModel(parts);
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        private readonly InventoryViewModel Parts;
        public ISet<ItemViewModel> ItemViewModels { get { return Parts.ItemViewModels; } }
        public double InventoryViewModelCost { get { return Parts.TotalCost; } }
        public double SaleProfit { get { return Price - InventoryViewModelCost; } }

        public int Count(ItemViewModel item)
        {
            return Parts.Count(item);
        }
    }
}
