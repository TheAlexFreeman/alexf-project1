using System;
using System.Collections.Generic;
using Xunit;

using WebStore.BLL;

namespace Garden.Tests
{
    public class LocationTests
    {
        [Fact]
        public void TestLocationConstructor()
        {
            // Arrange
            Location location;
            // Act
            location = new Location("Store1");
            // Assert
            Assert.Equal("Store1", location.Name);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void TestInventoryCount(int quantity)
        {
            // Arrange
            var location = new Location("TestLocation");
            var item = new Item();
            // Act
            location.AddToInventory(item, quantity);
            // Assert
            Assert.Equal(quantity, location.Count(item));
            location.AddToInventory(item);
            Assert.Equal(quantity + 1, location.Count(item));
        }

        [Fact]
        public void AddNegativeQuantityToInventory()
        {
            // Arrange
            Location location = new Location();
            Item item = new Item();
            // Act
            Action addNegativeOne = () => location.AddToInventory(item, -1);
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(addNegativeOne);
        }
        [Fact]
        public void AddNullToInventory()
        {
            // Arrange
            Location location = new Location("TestLocation");
            // Act
            Action addNull = () => location.IncrementInventory(null);
            // Assert
            Assert.Throws<ArgumentNullException>(addNull);
        }

        [Fact]
        public void SubtractSingleItemFromInventory()
        {
            // Arrange
            Location location = new Location("TestLocation");
            Item item = new Item("TestItem");
            location.IncrementInventory(item);
            // Act
            var enoughItems = location.DecrementInventory(item);
            // Assert
            Assert.True(enoughItems);
            Assert.Equal(0, location.Quantity(item));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void SubtractMultipleItemsFromInventory(int quantity)
        {
            // Arrange
            Location location = new Location("TestLocation");
            Item item = new Item("TestItem");
            location.AddToInventory(item, 2);
            // Act
            var enoughItems = location.SubtractFromInventory(item, quantity);
            // Assert
            Assert.True(enoughItems);
            Assert.Equal(2 - quantity, location.Quantity(item));
        }

        [Fact]
        public void SubtractTooManyItemsFromInventory()
        {
            // Arrange
            Location location = new Location("TestLocation");
            Item item = new Item("TestItem");
            location.IncrementInventory(item);
            // Act
            var enoughItems = location.SubtractFromInventory(item, 2);
            // Assert
            Assert.False(enoughItems);
        }
        [Fact]
        public void SubtractNegativeQuantityFromInventory()
        {
            // Arrange
            Location location = new Location("TestLocation");
            Item item = new Item("TestItem");
            // Act
            Action subtractNegativeOne = () => location.SubtractFromInventory(item, -1);
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(subtractNegativeOne);
        }
        [Fact]
        public void SubtractNonexistentItemFromInventory()
        {
            // Arrange
            Location location = new Location("TestLocation");
            // Act
            Action subtractNonexistent = () => location.DecrementInventory(new Item("TestItem"));
            // Assert
            Assert.Throws<KeyNotFoundException>(subtractNonexistent);
        }
        [Fact]
        public void SubtractNullFromInventory()
        {
            // Arrange
            Location location = new Location("TestLocation");
            // Act
            Action subtractNull = () => location.DecrementInventory(null);
            // Assert
            Assert.Throws<ArgumentNullException>(subtractNull);
        }

        private IDictionary<Item, int> MakeInventory(ref Location location)
        {
            // Test objects for testing order handling methods
            IDictionary<Item, int> inventory = new Dictionary<Item, int>();
            Item item1 = new Item("Item1", 1);
            Item item2 = new Item("Item2", 2);
            Item item3 = new Item("Item3", 3);
            inventory[item1] = 10;
            inventory[item2] = 20;
            inventory[item3] = 30;
            location.AddToInventory(inventory);
            return inventory;
        }
        private List<Product> MakeProducts()
        {
            IDictionary<Item, int> parts1 = new Dictionary<Item, int>();
            IDictionary<Item, int> parts2 = new Dictionary<Item, int>();
            IDictionary<Item, int> parts3 = new Dictionary<Item, int>();
            parts1[item1] = 1; parts1[item2] = 0; parts1[item3] = 1;
            parts2[item1] = 1; parts2[item2] = 2; parts2[item3] = 3;
            parts3[item1] = 1; parts3[item2] = 4; parts3[item3] = 9;
            Product product1 = new Product("Product1", "TestProduct", 5, parts1);
            Product product2 = new Product("Product2", "TestProduct", 25, parts2);
            Product product3 = new Product("Product3", "TestProduct", 125, parts3);
            return new List<Product>(new Product[] { product1, product2, product3 });
        }

        [Fact]
        public void FulfillSingleProductOrder()
        {
            // Arrange
            Location location = new Location("TestLocation");
            IDictionary<Item, int> productItems = new Dictionary<Item, int>();


        }
    }
}
