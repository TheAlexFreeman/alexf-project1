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
            var order3 = new Order(customer, location, time, DateTime.Now);
            // Assert
            Assert.Equal(customer, order1.Buyer);
            Assert.Equal(location, order1.Seller);
            Assert.Empty(order1.ProductSet);
            Assert.True(order2.ProductSet.SetEquals(products.Keys));
            Assert.Equal(time, order3.Start);
            Assert.Equal(order1.Start, order1.LastModified);
            Assert.True(order1.Start < order2.Start);
            Assert.True(order3.Start < order1.Start);
            Assert.True(order1.IsOpen);
            Assert.False(order3.IsOpen);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void TestOrderAddSubtract(int n)
        {
            var product = new Product();
            var order = new Order();
            Assert.Equal(0, order.Quantity(product));
            // Act
            order.AddProduct(product, n);
            Assert.Equal(n, order.Quantity(product));
            order.AddProduct(product);
            // Assert
            Assert.Equal(n + 1, order.Quantity(product));
            order.SubtractProduct(product, n);
            Assert.Equal(1, order.Quantity(product));
            order.RemoveProduct(product);
            Assert.Equal(0, order.Quantity(product));
            Assert.False(order.SubtractProduct(product));
        }

        [Theory]
        [InlineData(1, 1, 1, 1)]
        [InlineData(1, 0, 0, 1)]
        [InlineData(1, 2, 3, 4)]
        public void TestOrderTotal(double p1, double p2, int q1, int q2)
        {
            // Arrange
            var product1 = new Product("Product1", p1);
            var product2 = new Product("Product2", p2);
            var order = new Order();
            // Act
            order.AddProduct(product1, q1);
            // Assert
            Assert.Equal(p1 * q1, order.Total);
            order.AddProduct(product2, q2);
            Assert.Equal(p1 * q1 + p2 * q2, order.Total);
        }
    }
}
