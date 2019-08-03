using System;
using System.Collections.Generic;
using System.Text;
using WebStore.BLL;
using WebStore.BLL.Interfaces;

namespace WebStore.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public Customer GetCustomerById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetCustomersByLocation(int locationId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> SearchCustomersByName(string search)
        {
            throw new NotImplementedException();
        }
    }
}
