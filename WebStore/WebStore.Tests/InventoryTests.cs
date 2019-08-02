using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.BLL;
using Xunit;

namespace WebStore.Tests
{
    public class InventoryTests
    {
        [Fact]
        public void TestInventoryConstructor()
        {
            var items = new Dictionary<Item, int>();
            var item1 = new Item { Name = "TestItem1", Cost = 1 };
            var item2 = new Item { Name = "TestItem2", Cost = 0.5 };
            var item3 = new Item { Name = "TestItem3", Cost = 2 };
            items.Add(item1, 5);
            items.Add(item2, 10);
            var inventory = new Inventory(items);

            Assert.True(inventory.Items.SetEquals(items.Keys));
            Assert.Equal(5, inventory.Count(item1));
            Assert.Equal(10, inventory.Count(item2));
            Assert.Equal(0, inventory.Count(item3));
        }
        [Fact]
        public void TestInventoryCost()
        {
            var items = new Dictionary<Item, int>();
            var item1 = new Item { Name = "TestItem1", Cost = 1 };
            var item2 = new Item { Name = "TestItem2", Cost = 0.5 };
            var item3 = new Item { Name = "TestItem3", Cost = 2 };

            items.Add(item1, 5);
            items.Add(item2, 10);
            var inventory = new Inventory(items);
            Assert.Equal(5, inventory.ItemCost(item1));
            Assert.Equal(5, inventory.ItemCost(item2));
            Assert.Equal(0, inventory.ItemCost(item3));
            Assert.Equal(10, inventory.TotalCost);
        }
    }
}
