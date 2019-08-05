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

        public void AddCustomer(Customer customer)
        {
            if (customer.Id != 0)
            {
                throw new InvalidOperationException($"Customer with ID {customer.Id} already exists in database");
            }
            // Handle special case when customer has newly created BLL.Location as DefaultStore
            if (customer.DefaultStore?.Id == 0)
            {
                _dbContext.Location.Add(Mapper.Map(customer.DefaultStore));
                _dbContext.SaveChanges();
                customer.DefaultStore = Mapper.Map(_dbContext.Location.FirstOrDefault(l => l.Name == customer.DefaultStore.Name));
            }
            Entities.Customer entity = Mapper.Map(customer);
            entity.Id = 0;
            _dbContext.Add(entity);
        }

        public void DeleteCustomer(int id)
        {
            var entity = _dbContext.Customer.Find(id);
            _dbContext.Remove(entity);
        }

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

        public void UpdateCustomer(Customer customer)
        {
            if (customer.Id == 0)
            {
                throw new KeyNotFoundException($"Customer {customer.FullName} does not exist in database");
            }
            // Handle special case when customer has newly created BLL.Location as DefaultStore (not null but id == 0)
            if (customer.DefaultStore?.Id == 0)
            {
                var locationRepo = new LocationRepository(_dbContext);
                try
                {
                    customer.DefaultStore = locationRepo.GetLocationByName(customer.DefaultStore.Name);
                } catch (KeyNotFoundException)
                {
                    // If no location with the given name exists, then add it to DB
                    _dbContext.Location.Add(Mapper.Map(customer.DefaultStore));
                    _dbContext.SaveChanges();
                    customer.DefaultStore = locationRepo.GetLocationByName(customer.DefaultStore.Name);

                }
            }
            var currentEntity = _dbContext.Customer.Find(customer.Id);
            var newEntity = Mapper.Map(customer);
            _dbContext.Entry(currentEntity).CurrentValues.SetValues(newEntity);
        }
    }
}
