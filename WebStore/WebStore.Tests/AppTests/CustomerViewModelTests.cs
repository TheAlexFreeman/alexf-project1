using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

using WebStore.App.Models;
using WebStore.BLL;

namespace WebStore.Tests.AppTests
{
    public class CustomerViewModelTests
    {
        [Fact]
        public void TestConversionFromBLLCustomer()
        {
            // Arrange
            var location = new Location("StoreName");
            var customer = new Customer("First", "Last", location);
            // Act
            var viewModel = new CustomerViewModel(customer);
            var newCustomer = viewModel.AsCustomer;
            // Assert
            Assert.Equal(customer.Id, viewModel.Id);
            Assert.Equal(customer.FirstName, viewModel.FirstName);
            Assert.Equal(customer.LastName, viewModel.LastName);
            Assert.Equal(customer.FullName, viewModel.FullName);

            Assert.Equal(customer.Id, newCustomer.Id);
            Assert.Equal(customer.FirstName, newCustomer.FirstName);
            Assert.Equal(customer.LastName, newCustomer.LastName);

            Assert.Equal(customer.DefaultStore.Id, viewModel.DefaultStore.Id);
            Assert.Equal(customer.DefaultStore.Name, viewModel.DefaultStore.Name);

            Assert.Equal(customer.DefaultStore.Name, newCustomer.DefaultStore.Name);
            Assert.Equal(customer.DefaultStore.Id, newCustomer.DefaultStore.Id);
        }
    }
}
