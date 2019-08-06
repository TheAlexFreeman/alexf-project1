using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.BLL.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetCustomers(string search);
        Customer GetCustomerById(int id);
        Customer GetCustomerByName(string first, string last);
        //IEnumerable<Customer> SearchCustomersByName(string search);
        //IEnumerable<Customer> GetCustomersByLocation(int locationId);
        void Save();

        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int id);
        IEnumerable<Order> GetOrderHistory(int id);

    }
}
