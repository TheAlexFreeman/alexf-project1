using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebStore.BLL
{
    public class Order
    {
        public Order() { Products = new Dictionary<Product, int>(); Start = DateTime.Now; LastModified = Start; IsOpen = true; }
        //public Order(Customer buyer, Location seller, int id = 0)
        //{
        //    Id = id;
        //    Buyer = buyer ?? throw new ArgumentNullException(nameof(buyer), "Order buyer cannot be null");
        //    Seller = seller ?? throw new ArgumentNullException(nameof(buyer), "Order buyer cannot be null");
        //    Products = new Dictionary<Product, int>();
        //    Start = DateTime.Now;
        //    End = null;
        //}
        /// <summary>
        /// Creates a new order with buyer, seller, and existing set of products
        /// </summary>
        /// <param name="buyer">Customer buying this order</param>
        /// <param name="seller">Location where this order was sold</param>
        /// <param name="products">Dictionary mapping products to quantities in this order</param>
        /// <param name="id">Database ID of order</param>
        public Order(Customer buyer, Location seller, IDictionary<Product, int> products = null, int id = 0)
        {
            Id = id;
            Buyer = buyer ?? throw new ArgumentNullException();
            Seller = seller ?? throw new ArgumentNullException();
            Products = products ?? new Dictionary<Product, int>();
            Start = DateTime.Now;
            LastModified = Start;
            IsOpen = true;
        }
        /// <summary>
        /// Creates a new order with buyer, seller, and empty product set
        /// </summary>
        /// <param name="buyer">Customer who placed this order</param>
        /// <param name="seller">Location where this order was fulfilled</param>
        /// <param name="time">Date and time this order was placed</param>
        /// <param name="id">Database ID of order</param>
        public Order(Customer buyer, Location seller,  DateTime start, DateTime? lastModified = null, bool isOpen=true, IDictionary<Product, int> products = null, int id = 0)
        {
            Id = id;
            Buyer = buyer ?? throw new ArgumentNullException();
            Seller = seller ?? throw new ArgumentNullException();
            Products = products ?? new Dictionary<Product, int>();
            Start = start;
            if (lastModified == null || lastModified < start)
            {
                LastModified = start;
            }
            else
            {
                LastModified = (DateTime)lastModified;
            }
            IsOpen = isOpen;
        }
        /// <summary>
        /// Database ID of this order
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// Customer who placed order
        /// </summary>
        public Customer Buyer { get; }
        /// <summary>
        /// Location where order was fulfilled
        /// </summary>
        public Location Seller { get; }
        /// <summary>
        /// Date and time order was placed
        /// </summary>
        public DateTime Start { get; }
        public DateTime LastModified { get; set; }
        public bool IsOpen { get; set; }
        /// <summary>
        /// Set of products sold in order, with quantity of each
        /// </summary>
        private readonly IDictionary<Product, int> Products;
        /// <summary>
        /// Convenience property to list all products in order
        /// </summary>
        public ISet<Product> ProductSet => new HashSet<Product>(Products.Keys);
        /// <summary>
        /// Gets quantity of given product currently in order
        /// </summary>
        /// <param name="product">Product to count</param>
        /// <returns>Number of given product in order</returns>
        public int Quantity(Product product)
        {
            if (!Products.ContainsKey(product))
            {
                return 0;
            }
            return Products[product];
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
                    total += product.Price * Products[product];
                }
                return total;
            }
        }
        /// <summary>
        /// Cost to store of inventory items for all products ordered
        /// </summary>
        public double InventoryCost
        {
            get
            {
                double total = 0.0;
                foreach (var product in Products.Keys)
                {
                    total += product.InventoryCost * Products[product];
                }
                return total;
            }
        }
        /// <summary>
        /// Store's profit from all products in order
        /// </summary>
        public double Profit { get { return Total - InventoryCost; } }
        /// <summary>
        /// Inventory of items needed for all products in this order
        /// </summary>
        public Inventory ItemsNeeded
        {
            get
            {
                Inventory totals = new Inventory();
                foreach (Product product in Products.Keys)
                {
                    totals.AddInventory(product.ItemsNeeded(Quantity(product)));
                }
                return totals;
            }
        }
        /// <summary>
        /// Adds the given quantity of a product to this order
        /// </summary>
        /// <param name="product">Product to be added to this order</param>
        /// <param name="toAdd">Quantity of product to add</param>
        public int AddProduct(Product product, int toAdd)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Cannot add null to order.");
            }
            if (toAdd < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(toAdd), "Cannot add negative number of items to order.");
            }
            if (Products.ContainsKey(product))
            {
                Products[product] += toAdd;
            }
            else
            {
                Products.Add(product, toAdd);
            }
            LastModified = DateTime.Now;
            return Products[product];
        }
        /// <summary>
        /// Convenience method for adding one of some product
        /// </summary>
        /// <param name="product">Product to be added</param>
        public int AddProduct(Product product)
        {
            return AddProduct(product, 1);
        }
        /// <summary>
        /// Subtracts quantity of given product from order total
        /// </summary>
        /// <param name="product">Product to subtract</param>
        /// <param name="toSubtract">Quantity to subtract</param>
        /// <returns>True iff quantity could be subtracted.</returns>
        public bool SubtractProduct(Product product, int toSubtract)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Cannot subtract null from order.");
            }
            if (toSubtract < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(toSubtract), "Cannot subtract negative number of items from order.");
            }
            if (Quantity(product) < toSubtract)
            {
                return false;
            }
            Products[product] -= toSubtract;
            LastModified = DateTime.Now;
            return true;
        }
        public bool SubtractProduct(Product product)
        {
            return SubtractProduct(product, 1);
        }
        /// <summary>
        /// Removes product from order altogether
        /// </summary>
        /// <param name="product">Product to remove</param>
        public void RemoveProduct(Product product)
        {
            Products.Remove(product);
            LastModified = DateTime.Now;
        }

        public DateTime Close()
        {
            if (!IsOpen)
            {
                throw new InvalidOperationException("Cannot close order already closed.");
            }
            IsOpen = false;
            LastModified = DateTime.Now;
            return LastModified;
        }
    }
}
