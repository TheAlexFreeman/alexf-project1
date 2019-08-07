using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.BLL;
using Xunit;

namespace WebStore.Tests.BLLTests
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
        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(10, 1.5)]
        public void TestAddItem(int count, double cost)
        {
            // Arrange
            var inventory = new Inventory();
            var item = new Item { Name = "TestItem", Cost = cost };
            Assert.Equal(0, inventory.Count(item));
            Assert.Equal(0, inventory.ItemCost(item));
            // Act
            inventory.AddItem(item, count);
            // Assert
            Assert.Equal(count, inventory.Count(item));
            Assert.Equal(count * cost, inventory.ItemCost(item));
            inventory.AddItem(item, count);
            Assert.Equal(2 * count, inventory.Count(item));
            Assert.Equal(2 * count * cost, inventory.ItemCost(item));
        }
        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(5, 10, 0)]
        public void TestInventoryCost(int q1, int q2, int q3)
        {
            // Arrange
            var inventory = new Inventory();
            var item1 = new Item("Item1", 1);
            var item2 = new Item("Item2", 2);
            var item3 = new Item("Item3", 3);
            // Act
            inventory.AddItem(item1, q1);
            inventory.AddItem(item2, q2);
            inventory.AddItem(item3, q3);
            // Assert
            var total = q1 * item1.Cost + q2 * item2.Cost + q3 * item3.Cost;
            Assert.Equal(total, inventory.TotalCost);
        }
        [Theory]
        [InlineData(1, 3, 5, 7)]
        [InlineData(1, 0, 0, 1)]
        [InlineData(1, 2, 3, 4)]
        public void TestAddInventory(int q11, int q12, int q21, int q22)
        {
            // Arrange
            var inventory1 = new Inventory();
            var inventory2 = new Inventory();
            var item1 = new Item("Item1", 1);
            var item2 = new Item("Item2", 2);
            inventory1.AddItem(item1, q11);
            inventory1.AddItem(item2, q12);
            inventory2.AddItem(item1, q21);
            inventory2.AddItem(item2, q22);
            // Act
            inventory1.AddInventory(inventory2);
            // Assert
            Assert.Equal(q11 + q21, inventory1.Count(item1));
            Assert.Equal(q12 + q22, inventory1.Count(item2));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void TestSubtractItem(int q)
        {
            // Arrange
            var inventory = new Inventory();
            var item = new Item();
            inventory.AddItem(item, q);
            var subtracted = false;
            for (int i = 0; i < q;)
            {
                subtracted = inventory.SubtractItem(item);
                Assert.True(subtracted);
                Assert.Equal(q - ++i, inventory.Count(item));
            }
            subtracted = inventory.SubtractItem(item);
            Assert.False(subtracted);
            Assert.Equal(0, inventory.Count(item));
        }
        [Theory]
        [InlineData(1, 1, 1)]
        public void TestProductAvailable(int inStock, int numParts, int numProducts)
        {
            // Arrange
            var inventory = new Inventory();
            var item = new Item();
            inventory.AddItem(item, inStock);
            var parts = new Dictionary<Item, int>();
            parts.Add(item, numParts);
            var product = new Product("TestProduct", 1, parts);
            // Act
            var available = inventory.ProductAvailable(product, numProducts);
            // Assert
            Assert.True(available);
            var subtracted = inventory.SubtractItem(item, inStock);
            Assert.False(inventory.ProductAvailable(product));
        }
    }
}
