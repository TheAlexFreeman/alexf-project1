using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.BLL;
using WebStore.BLL.Interfaces;
using NLog;

namespace WebStore.Data.Repositories
{
    public class CustomerRepository : Repository, ICustomerRepository
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public CustomerRepository(Entities.StoreDBContext dbContext) : base(dbContext) { }

        public void AddCustomer(Customer customer)
        {
            if (customer.Id != 0)
            {
                _logger.Error(new InvalidOperationException(), $"Customer with ID {customer.Id} already exists in database");
                throw new InvalidOperationException($"Customer with ID {customer.Id} already exists in database");
            }
            // Handle special case when customer has newly created BLL.Location as DefaultStore
            if (customer.DefaultStore?.Id == 0)
            {
                _dbContext.Location.Add(Mapper.Map(customer.DefaultStore));
                _dbContext.SaveChanges();
                customer.DefaultStore = Mapper.Map(_dbContext.Location.FirstOrDefault(l => l.Name == customer.DefaultStore.Name));
            }
            _logger.Info($"Adding customer {customer.FullName} to database.");
            Entities.Customer entity = Mapper.Map(customer);
            entity.Id = 0;
            _dbContext.Add(entity);
        }

        public void DeleteCustomer(int id)
        {
            var entity = _dbContext.Customer.Find(id);
            if (entity == null)
            {
                _logger.Warn($"No customer with ID {id} exists in database: ignoring");
            } else
            {
                _logger.Info($"Removing customer with ID {id} from database");
                _dbContext.Remove(entity);

            }
        }

        public Customer GetCustomerById(int id)
        {
            var customer = _dbContext.Customer
                .Include(c => c.DefaultStore)
                .FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                _logger.Error(new KeyNotFoundException(), $"No customer with ID {id} exists in database");
                throw new KeyNotFoundException($"No customer with ID {id} exists in database");

            } else
            {
                _logger.Info($"Retrieving customer with ID {id} from database");
                return Mapper.Map(customer);

            }
        }

        public Customer GetCustomerByName(string first, string last)
        {
            var customer = _dbContext.Customer
                            .Include(c => c.DefaultStore)
                            .FirstOrDefault(c => c.FirstName == first && c.LastName == last);
            if (customer == null)
            {
                _logger.Error(new KeyNotFoundException(), $"No customer with Name {first} {last} exists in database");
                throw new KeyNotFoundException($"No customer with name {first} {last} exists in database");

            }
            else
            {
                _logger.Info($"Retrieving customer {first} {last} from database");
                return Mapper.Map(customer);
            }
        }

        public IEnumerable<Customer> GetCustomers(string search = "")
        {
            _logger.Info($"Retrieving customers with name containing {search}");
            return _dbContext.Customer.Where(c => c.FirstName.Contains(search) || c.LastName.Contains(search)).Select(Mapper.Map);
        }

        public IEnumerable<Customer> GetCustomersByLocation(int locationId)
        {
            return _dbContext.Customer
                .Include(c => c.DefaultStore)
                .Where(c => c.DefaultStoreId == locationId)
                .Select(Mapper.Map);
        }

        public IEnumerable<Order> GetOrderHistory(int id)
        {
            _logger.Info($"Retrieving order history for customer with ID {id}");
            return _dbContext.Order
                .Where(o => o.BuyerId == id)
                .Include(o => o.Seller)
                .Include(o => o.ProductOrder)
                .ThenInclude(po => po.Product)
                .ThenInclude(p => p.ProductItem)
                .ThenInclude(pi => pi.Item)
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
                _logger.Error(new KeyNotFoundException(), $"Customer {customer.FullName} does not exist in database");
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
            _logger.Info($"Updating values for customer with ID {customer.Id}");
            var currentEntity = _dbContext.Customer.Find(customer.Id);
            var newEntity = Mapper.Map(customer);
            _dbContext.Entry(currentEntity).CurrentValues.SetValues(newEntity);
        }
    }
}
