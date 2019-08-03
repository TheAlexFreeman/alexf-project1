﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WebStore.BLL;

namespace WebStore.App.Models
{
    public class LocationViewModel
    {
        public LocationViewModel() {}
        public LocationViewModel(string name, InventoryViewModel inv=null, IEnumerable<ProductViewModel> products=null, int id=0)
        {
            Id = id;
            Name = name;
            ProductViewModels = products?.ToList() ?? new List<ProductViewModel>();
            Inv = inv ?? new InventoryViewModel();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        private readonly InventoryViewModel Inv;
        public readonly List<ProductViewModel> ProductViewModels;
        //private readonly List<Customer> Customers;
        //private readonly List<Order> Orders;

        public ISet<ItemViewModel> ItemsInStock { get { return Inv.Items; } }

        public int Count(ItemViewModel item) { return Inv.Count(item); }
        
        public bool ProductAvailable(ProductViewModel product, int quantity)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "ProductViewModel cannot be null");
            }
            return Inv.ProductAvailable(product, quantity);
        }
        public bool ProductAvailable(ProductViewModel product)
        {
            return Inv.ProductAvailable(product, 1);
        }

        public int AddToInventory(ItemViewModel item, int toAdd)
        {
            return Inv.AddItem(item, toAdd);
        }
        public int AddToInventory(ItemViewModel item)
        {
            return Inv.AddItem(item);
        }

        public bool SubtractFromInventory(ItemViewModel item, int toSubtract)
        {
            return Inv.SubtractItem(item, toSubtract);
        }
        
    }
}