using System;
using System.Collections.Generic;
using System.Linq;
using Garden.DataAccess.Entities;
using Garden.Library.Interfaces;
using Garden.Library.Models;
namespace Garden.DataAccess
{
   public static class Mapper
   {
        /// <summary>
        /// Translates Item from DB entity to business logic model
        /// </summary>
        /// <param name="item">Representation of row in Item table</param>
        /// <returns>Item object from business logic library</returns>
        public static Library.Models.Item Map(Entities.Item item)
        {
             return new Library.Models.Item(item.Name, decimal.ToDouble(item.Cost), item.Id);
        }
        /// <summary>
        /// Translates Item from business logic model to DB entity
        /// </summary>
        /// <param name="item">Item object from business logic library</param>
        /// <returns>Representation of row in Item table</returns>
        public static Entities.Item Map(Library.Models.Item item)
        {
            return new Entities.Item
            {
                Id = item.Id,
                Name = item.Name,
                Cost = (decimal)item.Cost,
            };
        }
        /// <summary>
        /// Translates collection of InventoryItem rows from DB to business logic Inventory model
        /// </summary>
        /// <param name="inventory">Collection of rows from InventoryItem join table</param>
        /// <returns>Inventory object for Location in business logic library</returns>
        public static Inventory Map(IEnumerable<InventoryItem> inventory)
        {
            var result = new Inventory();
            foreach (var inventoryItem in inventory)
            {
                result.AddItem(Map(inventoryItem.Item), inventoryItem.Quantity);
            }
            return result;
        }
        /// <summary>
        /// Translates collection of ProductItem rows from DB to business logic Inventory model 
        /// </summary>
        /// <param name="parts">Collection of rows from ProductItem join table</param>
        /// <returns>Inventory object for Product in business logic library</returns>
        public static Inventory Map(IEnumerable<ProductItem> parts)
        {
            var result = new Inventory();
            foreach (var productItem in parts)
            {
                result.AddItem(Map(productItem.Item), productItem.Quantity);
            }
            return result;
        }
        /// <summary>
        /// Translates Location from DB entity to business logic model
        /// </summary>
        /// <param name="location">Representation of row in Location table</param>
        /// <returns>Location object for business logic library</returns>
        public static Library.Models.Location Map(Entities.Location location)
        {
            return new Library.Models.Location(location.Name, Map(location.InventoryItem), location.Id);
        }
        /// <summary>
        /// Translates Location from business logic model to DB entity
        /// </summary>
        /// <param name="location">Location object from business logic library</param>
        /// <returns>Representation of row in Location table</returns>
        public static Entities.Location Map(Library.Models.Location location)
        {
            var result = new Entities.Location
            {
                Id = location.Id,
                Name = location.StoreName,
            };
            // Add representation of all rows in InventoryItem table that refer to this row
            foreach(Library.Models.Item item in location.Inventory.Items)
            {
                result.InventoryItem.Add(new InventoryItem
                {
                    LocationId = location.Id,
                    ItemId = item.Id,
                    Quantity = location.Quantity(item),
                    Location = Map(location),
                    Item = Map(item)
                });
            }
            return result;
        }
        /// <summary>
        /// Translates Product from DB entity to business logic model
        /// </summary>
        /// <param name="product">Representation of row in Product table</param>
        /// <returns>Product object from business logic library</returns>
        public static Library.Models.Product Map(Entities.Product product)
        {
            return new Library.Models.Product(product.Name, "Product", decimal.ToDouble(product.Price), Map(product.ProductItem), product.Id);
        }
        /// <summary>
        /// Translates Product from business logic model to DB entity
        /// </summary>
        /// <param name="product">Product object from business logic library</param>
        /// <returns>Representation of row in Product table</returns>
        public static Entities.Product Map(Library.Models.Product product)
        {
            var result = new Entities.Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = (decimal)product.Price
            };
            // Add representation of all rows in ProductItem table
            foreach (Library.Models.Item item in product.Parts.Items)
            {
                result.ProductItem.Add(new ProductItem
                {
                    ProductId = product.Id,
                    ItemId = item.Id,
                });
            }
            return result;
        }
        /// <summary>
        /// Translates Customer from DB entity to business logic model
        /// </summary>
        /// <param name="customer">Representation of row in Customer table</param>
        /// <returns>Customer object from business logic library</returns>
        public static Library.Models.Customer Map(Entities.Customer customer)
        {
            return new Library.Models.Customer(customer.FirstName, customer.LastName, Map(customer.DefaultStore), customer.Id);
        }
        /// <summary>
        /// Translates Customer from business logic model to DB entity
        /// </summary>
        /// <param name="customer">Customer object from business logic library</param>
        /// <returns>Representation of row in Customer table</returns>
        public static Entities.Customer Map(Library.Models.Customer customer)
        {
            return new Entities.Customer
            {
                Id = customer.Id ?? 0,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
            };
        }
        /// <summary>
        /// Translates Order from DB entity to business logic model
        /// </summary>
        /// <param name="order">Representation of row in Order table</param>
        /// <returns>Order object from business logic library</returns>
        public static Library.Models.Order Map(Entities.Order order)
        {
            var result = new Library.Models.Order(Map(order.Buyer), Map(order.Seller), order.Time, order.Id);
            foreach (var productOrder in order.ProductOrder)
            {
                result.AddProduct(Map(productOrder.Product), productOrder.Quantity);
            }
            return result;
        }
        /// <summary>
        /// Translates Order from business logic model to DB entity
        /// </summary>
        /// <param name="order">Order object from business logic library</param>
        /// <returns>Representation of row in Order table</returns>
        public static Entities.Order Map(Library.Models.Order order)
        {
            var result = new Entities.Order
            {
                Id = order.Id ?? 0,
                Time = order.Time,
                BuyerId = order.Buyer.Id ?? 0,
                SellerId = order.Seller.Id,

                Buyer = Map(order.Buyer),
                Seller = Map(order.Seller)
            };
            // Add representation of each row in ProductOrder table
            foreach(Library.Models.Product product in order.ProductList)
            {
                result.ProductOrder.Add(new ProductOrder{
                    ProductId = product.Id,
                    OrderId = order.Id ?? 0,
                    Quantity = order.Quantity(product)
                });
            }
            return result;
        }
   }
}
