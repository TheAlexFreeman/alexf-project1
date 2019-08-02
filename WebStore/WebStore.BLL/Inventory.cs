using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebStore.BLL
{
    /// <summary>
    /// Convenience class to abstract away repetitive inventory operations.
    /// </summary>
    public class Inventory
    {
        /// <summary>
        /// Constructs empty inventory
        /// </summary>
        public Inventory()
        {
            _inventory = new Dictionary<Item, int>();
        }
        /// <summary>
        /// Constructs inventory from given quantities of items in dictionary
        /// </summary>
        /// <param name="inventory">Dictionary mapping items to quantities in inventory</param>
        public Inventory(IDictionary<Item, int> inventory)
        {
            _inventory = inventory;
        }
        /// <summary>
        /// Constructs inventory with quantity 0 of given items
        /// </summary>
        /// <param name="items">Set of items to put in inventory</param>
        public Inventory(IEnumerable<Item> items)
        {
            _inventory = new Dictionary<Item, int>();
            foreach (Item item in items)
            {
                _inventory.Add(item, 0);
            }
        }
        /// <summary>
        /// Dictionary mapping Item instances to the stocked quantity of each item.
        /// </summary>
        private readonly IDictionary<Item, int> _inventory;
        /// <summary>
        /// Convenience property to list all items in inventory
        /// </summary>
        public ISet<Item> Items
        {
            get { return new HashSet<Item>(_inventory.Keys); }
        }
        /// <summary>
        /// Calculates the total cost of replacing this collection of inventory items
        /// </summary>
        /// <returns>Sum of inventory items cost multiplied by quantity of each</returns>
        public double TotalCost
        {
            get { return Items.Select(ItemCost).Sum(); }
        }
        /// <summary>
        /// Looks up quantity of given item in stock
        /// </summary>
        /// <param name="item">Item to look up</param>
        /// <returns>Quantity of item in stock</returns>
        public int Count(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Cannot look up null value in inventory.");
            }
            if (!_inventory.ContainsKey(item))
            {
                return 0;
            }
            return _inventory[item];
        }
        /// <summary>
        /// Adds the specified quantity of a particular item to inventory,
        /// inserting the item as a new key if not already present.
        /// </summary>
        /// <param name="item">Item to be added</param>
        /// <param name="toAdd">Quantity to add</param>
        /// <returns>New total in stock</returns>
        public int AddItem(Item item, int toAdd)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Cannot add null to inventory.");
            }
            if (toAdd < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(toAdd), "Cannot add negative number of items to inventory.");
            }
            if (_inventory.ContainsKey(item))
            {
                _inventory[item] += toAdd;
            }
            else
            {
                _inventory.Add(item, toAdd);
            }
            return _inventory[item];
        }
        public int AddItem(Item item)
        {
            return AddItem(item, 1);
        }
        /// <summary>
        /// Adds multiple items from another inventory
        /// </summary>
        /// <param name="newInventory">Inventory to be added to this one</param>
        /// <returns>this</returns>
        public Inventory AddInventory(Inventory newInventory)
        {
            return AddInventory(newInventory, 1);
            //if (newInventory == null) { return this; }
            //var newItems = newInventory.Items;
            //foreach (Item item in newItems)
            //{
            //    AddItem(item, newInventory.Count(item));
            //}
            //return this;
        }
        /// <summary>
        /// Adds multiple items from another inventory
        /// </summary>
        /// <param name="newInventory">Inventory to be added to this one</param>
        /// <returns>this</returns>
        public Inventory AddInventory(Inventory newInventory, int toAdd)
        {
            if (newInventory == null) { return this; }
            var newItems = newInventory.Items;
            foreach (Item item in newItems)
            {
                AddItem(item, newInventory.Count(item) * toAdd);
            }
            return this;
        }
        /// <summary>
        /// Adds multiple items from another inventory
        /// </summary>
        /// <param name="newInventory">Inventory to be added to this one</param>
        /// <returns>this</returns>
        public Inventory AddInventory(IDictionary<Item, int> newInventory)
        {
            if (newInventory == null) { return this; }
            var newItems = newInventory.Keys;
            foreach (Item item in newItems)
            {
                AddItem(item, newInventory[item]);
            }
            return this;
        }
        /// <summary>
        /// Checks whether the given quantity of some item is present
        /// </summary>
        /// <param name="item">Item to check against inventory</param>
        /// <param name="quantity">Quantity of item desired</param>
        /// <returns>True iff inventory has enough items</returns>
        public bool ItemAvailable(Item item, int quantity)
        {
            return Count(item) >= quantity;
        }
        /// <summary>
        /// Checks for enough items to create one of some product
        /// </summary>
        /// <param name="product">Product to check against inventory</param>
        /// <returns>True iff product can be made from inventory</returns>
        public bool ProductAvailable(Product product, int numProducts)
        {
            var productParts = product.Items;
            foreach (Item part in productParts)
            {
                if (!ItemAvailable(part, product.Count(part) * numProducts))
                {
                    return false;
                }
            }
            return true;
        }
        public bool ProductAvailable(Product product)
        {
            return ProductAvailable(product, 1);
        }
        /// <summary>
        /// Subtracts given quantity of some item from inventory
        /// </summary>
        /// <param name="item">Item to subtract</param>
        /// <param name="toSubtract">Quantity to subtract</param>
        /// <returns>True iff Inventory has enough items to remove</returns>
        public bool SubtractItem(Item item, int toSubtract)
        {
            if (Count(item) < toSubtract)
            {
                return false;
            }
            _inventory[item] -= toSubtract;
            return true;
        }
        public bool SubtractItem(Item item)
        {
            return SubtractItem(item, 1);
        }
        public double ItemCost(Item item)
        {
            return item.Cost * Count(item);
        }
        
        public bool MakeProduct(Product product, int quantity)
        {
            // Check availability before subtracting from stock
            if (!ProductAvailable(product, quantity))
            {
                return false;
            }
            foreach(Item item in product.Items)
            {
                SubtractItem(item, product.Count(item) * quantity);
            }
            return true;
        }
    }
}
