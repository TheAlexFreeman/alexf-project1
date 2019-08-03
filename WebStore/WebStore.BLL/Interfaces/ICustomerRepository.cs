using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.BLL.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetCustomers();
        Customer GetCustomerById(int id);
        Customer GetCustomerByName(string first, string last);
        IEnumerable<Customer> SearchCustomersByName(string search);
        IEnumerable<Customer> GetCustomersByLocation(int locationId);
    }
}
