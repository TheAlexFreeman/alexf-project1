using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

using WebStore.App.Models;
using WebStore.BLL;

namespace WebStore.Tests.AppTests
{
    public class LocationViewModelTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void TestConversionFromBLLLocation(int numItems)
        {
            // Arrange
            var items = new Dictionary<Item, int>();
            var products = new HashSet<Product>();
            for (int i = 0; i < numItems; i++)
            {
                items.Add(new Item($"Item{i}", i + 1), i);
                products.Add(new Product($"Product{i}", i * 2));
            }
            var inventory = new Inventory(items);
            var location = new Location("StoreName", inventory, products);
            // Act
            var viewModel = new LocationViewModel(location);
            var newLocation = viewModel.AsLocation;
            // Assert
            Assert.Equal(location.Id, viewModel.Id);
            Assert.Equal(location.Name, viewModel.Name);

            Assert.Equal(location.Id, newLocation.Id);
            Assert.Equal(location.Name, newLocation.Name);
            //Assert.True(location.Products.SetEquals(newLocation.Products));
            //Assert.True(location.ItemsInStock.SetEquals(newLocation.ItemsInStock));
        }
    }
}
