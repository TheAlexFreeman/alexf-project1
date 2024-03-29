﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebStore.BLL
{
    public class Location
    {
        public Location() { Inv = new Inventory(); Products = new HashSet<Product>(); }
        public Location(string name, Inventory inv=null, ISet<Product> products=null, int id=0)
        {
            Id = id;
            Name = name;
            Products = products ?? new HashSet<Product>();
            Inv = inv ?? new Inventory();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        private readonly Inventory Inv;
        public readonly ISet<Product> Products;

        //private readonly List<Customer> Customers;
        //private readonly List<Order> Orders;

        public ISet<Item> ItemsInStock { get { return Inv.Items; } }

        public int Count(Item item) { return Inv.Count(item); }
        
        public bool ProductAvailable(Product product, int quantity)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null");
            }
            if (!Products.Contains(product))
            {
                return false;
            }
            return Inv.ProductAvailable(product, quantity);
        }
        public bool ProductAvailable(Product product)
        {
            return ProductAvailable(product, 1);
        }

        public int AddToInventory(Item item, int toAdd)
        {
            return Inv.AddItem(item, toAdd);
        }
        public int AddToInventory(Item item)
        {
            return Inv.AddItem(item);
        }
        public Inventory AddInventory(Inventory newInventory)
        {
            return Inv.AddInventory(newInventory);
        }

        public bool SubtractFromInventory(Item item, int toSubtract)
        {
            return Inv.SubtractItem(item, toSubtract);
        }
        public bool SubtractInventory(Inventory toSubtract)
        {
            return Inv.SubtractInventory(toSubtract);
        }

        public bool FulfillOrder(Order order)
        {
            if (!Inv.HasEnough(order.ItemsNeeded))
            {
                return false;
            }
            return SubtractInventory(order.ItemsNeeded);
        }
    }
}
