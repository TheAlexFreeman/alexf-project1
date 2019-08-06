using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WebStore.BLL;

namespace WebStore.App.Models
{
    public class OrderViewModel
    {
        public OrderViewModel(Order order)
        {
            Id = order.Id;
            Buyer = new CustomerViewModel(order.Buyer);
            Seller = new LocationViewModel(order.Seller);
            Start = order.Start;
            LastModified = order.LastModified;
            IsOpen = order.IsOpen;
            Products = new Dictionary<string, int>();
            Prices = new Dictionary<string, double>();

            foreach (Product product in order.ProductSet)
            {
                Products.Add(product.Name, order.Quantity(product));
                Prices.Add(product.Name, product.Price);
            }
        }

        public OrderViewModel() { }


        /// <summary>
        /// Database ID of this order
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// CustomerViewModel who placed order
        /// </summary>
        public CustomerViewModel Buyer { get; }
        /// <summary>
        /// LocationViewModel where order was fulfilled
        /// </summary>
        public LocationViewModel Seller { get; }
        /// <summary>
        /// Date and time order was placed
        /// </summary>
        public DateTime Start { get; }
        public DateTime LastModified { get; set; }
        public bool IsOpen { get; set; }
        /// <summary>
        /// Names of products sold, with quantity of each
        /// </summary>
        public readonly IDictionary<string, int> Products;
        /// <summary>
        /// Prices of all products sold, indexed by product name 
        /// </summary>
        public IDictionary<string, double> Prices;
        /// <summary>
        /// Gets quantity of given product currently in order
        /// </summary>
        /// <param name="product">Product to count</param>
        /// <returns>Number of given product in order</returns>
        public int Quantity(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentNullException("Product name cannot be empty");
            }
            if (!Products.ContainsKey(productName))
            {
                return 0;
            }
            return Products[productName];
        }
        /// <summary>
        /// Total price of this order, calculated from product prices
        /// </summary>
        public double Total
        {
            get
            {
                return Products.Select(kvp => kvp.Value * Prices[kvp.Key]).Sum();
                //double total = 0.0;
                //foreach (var product in Products.Keys)
                //{
                //    total += Prices[product] * Products[product];
                //}
                //return total;
            }
        }
        
        public DateTime Close()
        {
            if (!IsOpen)
            {
                throw new InvalidOperationException("Order is already closed.");
            }
            IsOpen = false;
            LastModified = DateTime.Now;
            return LastModified;
        }


        //public Order AsOrder
        //{
        //    get
        //    {
        //        return new Order(Buyer.AsCustomer, Seller.AsLocation, Start, LastModified, IsOpen,
        //            new Dictionary<Product, int>(Products.Select(kvp => new KeyValuePair<Product, int>(kvp.Key.AsProduct, kvp.Value))), Id);
        //    }
        //}
    }
}
