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
    }
}
