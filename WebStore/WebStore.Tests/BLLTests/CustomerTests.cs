using System;
using System.Collections.Generic;
using System.Text;
using WebStore.BLL;
using Xunit;

namespace WebStore.Tests.BLLTests
{
    public class CustomerTests
    {
        [Fact]
        public void TestCustomerConstructor()
        {
            // Arrange
            Customer customer;
            // Act
            customer = new Customer("Alex", "Freeman");
            // Assert
            Assert.Equal("Alex", customer.FirstName);
            Assert.Equal("Freeman", customer.LastName);
            Assert.Equal("Alex Freeman", customer.FullName);
        }

        [Fact]
        public void InitializeCustomerWithoutLocation()
        {
            Customer customer = new Customer("John", "Doe");
            Assert.Null(customer.DefaultStore);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetCustomerDefaultLocation(int numLocations)
        {
            // Arrange 
            Customer customer = new Customer("John", "Doe");
            List<Location> locations = new List<Location>();
            // Act
            for (int i = 0; i < numLocations; i++)
            {
                locations.Add(new Location("Store #" + i.ToString()));
                customer.DefaultStore = locations[i];
            }
            // Assert
            Assert.Equal(locations[numLocations - 1], customer.DefaultStore);
        }
    }
}
