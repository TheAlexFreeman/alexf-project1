using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.BLL;

namespace WebStore.App.Models
{
    /// <summary>
    /// Convenience class to abstract away repetitive inventory operations.
    /// </summary>
    public class InventoryViewModel
    {
        public InventoryViewModel(Inventory inventory)
        {
            _inventory = new Dictionary<ItemViewModel, int>();

            foreach (Item item in inventory.Items)
            {
                _inventory.Add(new ItemViewModel(item), inventory.Count(item));
            }
        }
        /// <summary>
        /// Constructs empty inventory
        /// </summary>
        public InventoryViewModel()
        {
            _inventory = new Dictionary<ItemViewModel, int>();
        }
        /// <summary>
        /// Constructs inventory from given quantities of items in dictionary
        /// </summary>
        /// <param name="inventory">Dictionary mapping items to quantities in inventory</param>
        public InventoryViewModel(IDictionary<ItemViewModel, int> inventory)
        {
            _inventory = inventory;
        }
        /// <summary>
        /// Constructs inventory with quantity 0 of given items
        /// </summary>
        /// <param name="items">Set of items to put in inventory</param>
        public InventoryViewModel(IEnumerable<ItemViewModel> items)
        {
            _inventory = new Dictionary<ItemViewModel, int>();
            foreach (ItemViewModel item in items)
            {
                _inventory.Add(item, 0);
            }
        }
        /// <summary>
        /// Dictionary mapping ItemViewModel instances to the stocked quantity of each item.
        /// </summary>
        private readonly IDictionary<ItemViewModel, int> _inventory;
        /// <summary>
        /// Convenience property to list all items in inventory
        /// </summary>
        public ISet<ItemViewModel> Items
        {
            get { return new HashSet<ItemViewModel>(_inventory.Keys); }
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
        /// <param name="item">ItemViewModel to look up</param>
        /// <returns>Quantity of item in stock</returns>
        public int Count(ItemViewModel item)
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
        /// <param name="item">ItemViewModel to be added</param>
        /// <param name="toAdd">Quantity to add</param>
        /// <returns>New total in stock</returns>
        public int AddItem(ItemViewModel item, int toAdd)
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
        public int AddItem(ItemViewModel item)
        {
            return AddItem(item, 1);
        }
        /// <summary>
        /// Adds multiple items from another inventory
        /// </summary>
        /// <param name="newInventoryViewModel">InventoryViewModel to be added to this one</param>
        /// <returns>this</returns>
        public InventoryViewModel AddInventory(InventoryViewModel newInventoryViewModel)
        {
            return AddInventory(newInventoryViewModel, 1);
            //if (newInventoryViewModel == null) { return this; }
            //var newItemViewModels = newInventoryViewModel.ItemViewModels;
            //foreach (ItemViewModel item in newItemViewModels)
            //{
            //    AddItemViewModel(item, newInventoryViewModel.Count(item));
            //}
            //return this;
        }
        /// <summary>
        /// Adds multiple items from another inventory
        /// </summary>
        /// <param name="newInventoryViewModel">InventoryViewModel to be added to this one</param>
        /// <returns>this</returns>
        public InventoryViewModel AddInventory(InventoryViewModel newInventoryViewModel, int toAdd)
        {
            if (newInventoryViewModel == null) { return this; }
            var newItemViewModels = newInventoryViewModel.Items;
            foreach (ItemViewModel item in newItemViewModels)
            {
                AddItem(item, newInventoryViewModel.Count(item) * toAdd);
            }
            return this;
        }
        /// <summary>
        /// Adds multiple items from another inventory
        /// </summary>
        /// <param name="newInventoryViewModel">InventoryViewModel to be added to this one</param>
        /// <returns>this</returns>
        public InventoryViewModel AddInventory(IDictionary<ItemViewModel, int> newInventoryViewModel)
        {
            if (newInventoryViewModel == null) { return this; }
            var newItemViewModels = newInventoryViewModel.Keys;
            foreach (ItemViewModel item in newItemViewModels)
            {
                AddItem(item, newInventoryViewModel[item]);
            }
            return this;
        }
        /// <summary>
        /// Checks whether the given quantity of some item is present
        /// </summary>
        /// <param name="item">ItemViewModel to check against inventory</param>
        /// <param name="quantity">Quantity of item desired</param>
        /// <returns>True iff inventory has enough items</returns>
        public bool ItemAvailable(ItemViewModel item, int quantity)
        {
            return Count(item) >= quantity;
        }
        /// <summary>
        /// Checks for enough items to create one of some product
        /// </summary>
        /// <param name="product">Product to check against inventory</param>
        /// <returns>True iff product can be made from inventory</returns>
        public bool ProductAvailable(ProductViewModel product, int numProducts)
        {
            var productParts = product.Items;
            foreach (ItemViewModel part in productParts)
            {
                if (!ItemAvailable(part, product.Count(part) * numProducts))
                {
                    return false;
                }
            }
            return true;
        }
        public bool ProductAvailable(ProductViewModel product)
        {
            return ProductAvailable(product, 1);
        }
        /// <summary>
        /// Subtracts given quantity of some item from inventory
        /// </summary>
        /// <param name="item">ItemViewModel to subtract</param>
        /// <param name="toSubtract">Quantity to subtract</param>
        /// <returns>True iff InventoryViewModel has enough items to remove</returns>
        public bool SubtractItem(ItemViewModel item, int toSubtract)
        {
            if (Count(item) < toSubtract)
            {
                return false;
            }
            _inventory[item] -= toSubtract;
            return true;
        }
        public bool SubtractItem(ItemViewModel item)
        {
            return SubtractItem(item, 1);
        }
        public double ItemCost(ItemViewModel item)
        {
            return item.Cost * Count(item);
        }
        
        public bool MakeProduct(ProductViewModel product, int quantity)
        {
            // Check availability before subtracting from stock
            if (!ProductAvailable(product, quantity))
            {
                return false;
            }
            foreach(ItemViewModel item in product.Items)
            {
                SubtractItem(item, product.Count(item) * quantity);
            }
            return true;
        }


        public Inventory AsInventory
        {
            get
            {
                return new Inventory(new Dictionary<Item, int>(_inventory.Select(kvp => new KeyValuePair<Item, int>(kvp.Key.AsItem, kvp.Value))));
            }
        }
    }
}
