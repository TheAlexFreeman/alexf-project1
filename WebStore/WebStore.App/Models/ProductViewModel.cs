using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.BLL;

namespace WebStore.App.Models
{
    public class ProductViewModel
    {
        public ProductViewModel(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Price = product.Price;
            Parts = new Dictionary<string, int>(product.Items.Select(i => new KeyValuePair<string, int>(i.Name, product.Count(i))));
            InventoryCosts = new Dictionary<string, double>(product.Items.Select(i => new KeyValuePair<string, double>(i.Name, i.Cost)));
        }
        public ProductViewModel() { }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        private readonly Dictionary<string, int> Parts;
        private readonly Dictionary<string, double> InventoryCosts;

        public double SaleProfit => Price - InventoryCosts.Select(kvp => kvp.Value * Count(kvp.Key)).Sum();

        public int Count(string item)
        {
            return Parts[item];
        }

        public Inventory ItemsNeeded(int quantity)
        {
            var result = new Inventory();
            foreach (string itemName in Parts.Keys)
            {
                result.AddItem(new Item(itemName, InventoryCosts[itemName]), Count(itemName) * quantity);
            }
            return result;
        }


        public Product AsProduct
        {
            get
            {
                return new Product(Name, Price, ItemsNeeded(1), Id);
            }
        }
    }
}
