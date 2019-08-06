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
            foreach(Product product in order.Seller.Products)
            {
                Products.Add(product.Name, order.Quantity(product));
            }
        }

        public OrderViewModel() { }

        //public OrderViewModel() { Products = new Dictionary<string, int>(); Start = DateTime.Now; LastModified = Start; IsOpen = true; }
        ///// <summary>
        ///// Creates a new order with buyer, seller, and existing set of products
        ///// </summary>
        ///// <param name="buyer">CustomerViewModel buying this order</param>
        ///// <param name="seller">LocationViewModel where this order was sold</param>
        ///// <param name="products">Dictionary mapping products to quantities in this order</param>
        ///// <param name="id">Database ID of order</param>
        //public OrderViewModel(CustomerViewModel buyer, LocationViewModel seller, IDictionary<ProductViewModel, int> products = null, int id = 0)
        //{
        //    Id = id;
        //    Buyer = buyer ?? throw new ArgumentNullException();
        //    Seller = seller ?? throw new ArgumentNullException();
        //    if (products == null)
        //    {
        //        Products = new Dictionary<string, int>();
        //        Prices = new Dictionary<string, double>();
        //    } else
        //    {
        //        Products = new Dictionary<string, int>(products.Select(kvp => new KeyValuePair<string, int>(kvp.Key.Name, kvp.Value)));
        //        Prices = new Dictionary<string, double>(products.Select(kvp => new KeyValuePair<string, double>(kvp.Key.Name, kvp.Key.Price)));
        //    }
        //    Start = DateTime.Now;
        //    LastModified = Start;
        //}
        ///// <summary>
        ///// Creates a new order with buyer, seller, and empty product set
        ///// </summary>
        ///// <param name="buyer">CustomerViewModel who placed this order</param>
        ///// <param name="seller">LocationViewModel where this order was fulfilled</param>
        ///// <param name="time">Date and time this order was placed</param>
        ///// <param name="id">Database ID of order</param>
        //public OrderViewModel(CustomerViewModel buyer, LocationViewModel seller, DateTime start, DateTime? lastModified = null, IDictionary<ProductViewModel, int> products = null, int id = 0)
        //{
        //    Id = id;
        //    Buyer = buyer ?? throw new ArgumentNullException();
        //    Seller = seller ?? throw new ArgumentNullException();
        //    if (products == null)
        //    {
        //        Products = new Dictionary<string, int>();
        //        Prices = new Dictionary<string, double>();
        //    }
        //    else
        //    {
        //        Products = new Dictionary<string, int>(products.Select(kvp => new KeyValuePair<string, int>(kvp.Key.Name, kvp.Value)));
        //        Prices = new Dictionary<string, double>(products.Select(kvp => new KeyValuePair<string, double>(kvp.Key.Name, kvp.Key.Price)));
        //    }
        //    Start = start;
        //    if (lastModified == null || lastModified < start)
        //    {
        //        LastModified = start;
        //    } else
        //    {
        //        LastModified = (DateTime)lastModified;
        //    }
        //}


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
        private readonly IDictionary<string, int> Products;
        /// <summary>
        /// Prices of all products sold, indexed by product name 
        /// </summary>
        public IDictionary<string, double> Prices => Seller?.Prices;
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
                double total = 0.0;
                foreach (var product in Products.Keys)
                {
                    total += Prices[product] * Products[product];
                }
                return total;
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
