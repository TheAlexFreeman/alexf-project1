using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.BLL;

namespace WebStore.Data
{
    public static class Mapper
    {
        /// <summary>
        /// Translates Item from DB entity to business logic model
        /// </summary>
        /// <param name="item">Representation of row in Item table</param>
        /// <returns>Item object from business logic library</returns>
        public static Item Map(Entities.Item item)
        {
            return new Item(item.Name, decimal.ToDouble(item.Cost), item.Id);
        }
        /// <summary>
        /// Translates Item from business logic model to DB entity
        /// </summary>
        /// <param name="item">Item object from business logic library</param>
        /// <returns>Representation of row in Item table</returns>
        public static Entities.Item Map(Item item)
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
        public static Inventory Map(IEnumerable<Entities.InventoryItem> inventory)
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
        public static Inventory Map(IEnumerable<Entities.ProductItem> parts)
        {
            var result = new Inventory();
            foreach (var productItem in parts)
            {
                result.AddItem(Map(productItem.Item), productItem.Quantity);
            }
            return result;
        }
        public static ISet<Product> Map(IEnumerable<Entities.ProductLocation> products)
        {
            var result = new HashSet<Product>();
            foreach(var productLocation in products)
            {
                result.Add(Map(productLocation.Product));
            }
            return result;
        }
        /// <summary>
        /// Translates Location from DB entity to business logic model
        /// </summary>
        /// <param name="location">Representation of row in Location table</param>
        /// <returns>Location object for business logic library</returns>
        public static Location Map(Entities.Location location)
        {
            if (location == null) { return null; }
            return new Location(location.Name, Map(location.InventoryItem), Map(location.ProductLocation), location.Id);
        }
        /// <summary>
        /// Translates Location from business logic model to DB entity
        /// </summary>
        /// <param name="location">Location object from business logic library</param>
        /// <returns>Representation of row in Location table</returns>
        public static Entities.Location Map(Location location)
        {
            if (location == null) { return null; }
            var result = new Entities.Location
            {
                Id = location.Id,
                Name = location.Name,
            };
            // Add representation of all rows in InventoryItem table that refer to this row
            foreach (Item item in location.ItemsInStock)
            {
                result.InventoryItem.Add(new Entities.InventoryItem
                {
                    Quantity = location.Count(item),
                    Location = result,
                    Item = Map(item)
                });
            }
            foreach(Product product in location.Products)
            {
                result.ProductLocation.Add(new Entities.ProductLocation
                {
                    Product = Map(product),
                    Location = Map(location)
                });
            }
            return result;
        }
        /// <summary>
        /// Translates Product from DB entity to business logic model
        /// </summary>
        /// <param name="product">Representation of row in Product table</param>
        /// <returns>Product object from business logic library</returns>
        public static Product Map(Entities.Product product)
        {
            return new Product(product.Name, decimal.ToDouble(product.Price), Map(product.ProductItem), product.Id);
        }
        /// <summary>
        /// Translates Product from business logic model to DB entity
        /// </summary>
        /// <param name="product">Product object from business logic library</param>
        /// <returns>Representation of row in Product table</returns>
        public static Entities.Product Map(Product product)
        {
            var result = new Entities.Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = (decimal)product.Price
            };
            // Add representation of all rows in ProductItem table
            foreach (Item item in product.Items)
            {
                result.ProductItem.Add(new Entities.ProductItem
                {
                    //ProductId = product.Id,
                    //ItemId = item.Id,
                    Product = result,
                    Item = Map(item),
                    Quantity = product.Count(item)
                });
            }
            return result;
        }
        /// <summary>
        /// Translates Customer from DB entity to business logic model
        /// </summary>
        /// <param name="customer">Representation of row in Customer table</param>
        /// <returns>Customer object from business logic library</returns>
        public static Customer Map(Entities.Customer customer)
        {
            if (customer == null) { return null; }
            return new Customer(customer.FirstName, customer.LastName, Map(customer.DefaultStore), customer.Id);
        }
        /// <summary>
        /// Translates Customer from business logic model to DB entity
        /// </summary>
        /// <param name="customer">Customer object from business logic library</param>
        /// <returns>Representation of row in Customer table</returns>
        public static Entities.Customer Map(Customer customer)
        {
            if (customer == null) { return null; }
            return new Entities.Customer
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                DefaultStore = Map(customer.DefaultStore)
            };
        }
        public static Dictionary<Product, int> Map(ICollection<Entities.ProductOrder> productOrders)
        {
            var result = new Dictionary<Product, int>();
            foreach (var productOrder in productOrders)
            {
                result.Add(Map(productOrder.Product), productOrder.Quantity);
            }
            return result;
        }
        /// <summary>
        /// Translates Order from DB entity to business logic model
        /// </summary>
        /// <param name="order">Representation of row in Order table</param>
        /// <returns>Order object from business logic library</returns>
        public static Order Map(Entities.Order order)
        {
            DateTime? end = null;
            if (order.IsOpen) { end = order.LastModified; }
            var result = new Order(Map(order.Buyer), Map(order.Seller), order.Start, order.LastModified, order.IsOpen, Map(order.ProductOrder), order.Id);
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
        public static Entities.Order Map(Order order)
        {
            var result = new Entities.Order
            {
                Id = order.Id,
                Start = order.Start,
                LastModified = order.LastModified,
                IsOpen = order.IsOpen,
                Buyer = Map(order.Buyer),
                Seller = Map(order.Seller),
                ProductOrder = new List<Entities.ProductOrder>()
            };
            // Add representation of each row in ProductOrder table
            foreach (Product product in order.ProductSet)
            {
                result.ProductOrder.Add(new Entities.ProductOrder
                {
                    Product = Map(product),
                    Order = result,
                    Quantity = order.Quantity(product)
                });
            }
            return result;
        }
    }
}
