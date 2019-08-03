using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.BLL.Interfaces
{
    public interface ICustomerRepository
    {
        Customer GetCustomerById(int id);
        IEnumerable<Customer> SearchCustomersByName(string search);
        IEnumerable<Customer> GetCustomersByLocation(int locationId);
    }
}
