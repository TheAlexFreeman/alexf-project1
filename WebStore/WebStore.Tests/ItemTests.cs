using System;
using WebStore.BLL;
using Xunit;

namespace WebStore.Tests
{
    public class ItemTests
    {
        [Fact]
        public void TestItemConstructor()
        {
            Item item;
            item = new Item("TestItem", 1);
            Assert.Equal(0, item.Id);
            Assert.Equal("TestItem", item.Name);
            Assert.Equal(1.0, item.Cost);
        }

    }
}
