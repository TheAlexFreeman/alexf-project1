using System;
using System.Collections.Generic;
using System.Text;
using WebStore.BLL;
using Xunit; 

namespace WebStore.Tests
{
    public class ProductTests
    {
        [Fact]
        public void TestProductConstructor()
        {
            var product = new Product("TestProduct", 1.5);
            
            Assert.Equal(0, product.Id);
            Assert.Equal("TestProduct", product.Name);
            Assert.Equal(1.5, product.Price);
            Assert.True(product.Items.SetEquals(new HashSet<Item>()));
        }
        [Fact]
        public void TestProductItems()
        {
            var item = new Item();
            var parts = new Dictionary<Item, int>();
            parts.Add(item, 1);
            var product = new Product("TestProduct", 1.5, parts);

            Assert.True(product.Items.SetEquals(parts.Keys));
            Assert.Equal(1, product.Count(item));
        }
        [Fact]
        public void TestProductCostAndProfit()
        {
            var item1 = new Item("TestItem1", 1);
            var parts = new Dictionary<Item, int>();
            parts.Add(item1, 2);
            var product = new Product("TestProduct", 2.5, parts);

            Assert.Equal(2, product.InventoryCost);
            Assert.Equal(0.5, product.SaleProfit);

            var item2 = new Item("TestItem2", 0.5);
            parts.Add(item2, 1);
            Assert.Equal(0, product.SaleProfit);
        }

        
    }
}
