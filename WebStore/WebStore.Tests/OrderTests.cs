using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

using WebStore.BLL;

namespace WebStore.Tests
{
    public class OrderTests
    {
        [Fact]
        public void TestOrderConstructor()
        {
            // Arrange
            var location = new Location();
            var item = new Item("TestItem", 1);
            location.AddToInventory(item, 10);
            var parts = new Dictionary<Item, int>();
            parts.Add(item, 2);
            var product = new Product("TestProduct", 2, parts);
            var products = new Dictionary<Product, int>();
            products.Add(product, 2);
            var customer = new Customer();
            var time = DateTime.Now;
            // Act
            var order1 = new Order(customer, location);
            var order2 = new Order(customer, location, products);
            var order3 = new Order(customer, location, time);
            // Assert
            Assert.Equal(customer, order1.Buyer);
            Assert.Equal(location, order1.Seller);
            Assert.Empty(order1.ProductSet);
            Assert.True(order2.ProductSet.SetEquals(products.Keys));
            Assert.Equal(time, order3.Start);
            Assert.True(order1.Start < order2.Start);
            Assert.True(order3.Start < order1.Start);
        }
    }
}
