using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.BLL;
using WebStore.BLL.Interfaces;

namespace WebStore.Data.Repositories
{
    public class CustomerRepository : Repository, ICustomerRepository
    {
        public CustomerRepository(Entities.StoreDBContext dbContext) : base(dbContext) { }
        public Customer GetCustomerById(int id)
        {
            var customer = _dbContext.Customer
                .Include(c => c.DefaultStore)
                .FirstOrDefault(c => c.Id == id)
                ?? throw new KeyNotFoundException($"No customer with ID {id} exists in database");
            return Mapper.Map(customer);
        }

        public Customer GetCustomerByName(string first, string last)
        {
            var customer = _dbContext.Customer
                            .Include(c => c.DefaultStore)
                            .FirstOrDefault(c => c.FirstName == first && c.LastName == last)
                            ?? throw new KeyNotFoundException($"No customer with name '{first} {last}' exists in database");
            return Mapper.Map(customer);
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _dbContext.Customer.Select(Mapper.Map);
        }

        public IEnumerable<Customer> GetCustomersByLocation(int locationId)
        {
            return _dbContext.Customer
                .Include(c => c.DefaultStore)
                .Where(c => c.DefaultStoreId == locationId)
                .Select(Mapper.Map);
        }

        public IEnumerable<Customer> SearchCustomersByName(string search)
        {
            return _dbContext.Customer.Select(Mapper.Map).Where(c => c.FullName.Contains(search));
        }
    }
}
