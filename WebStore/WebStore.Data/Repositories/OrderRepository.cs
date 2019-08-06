using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.BLL;
using WebStore.BLL.Interfaces;

namespace WebStore.Data.Repositories
{
    public class OrderRepository : Repository, IOrderRepository
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public OrderRepository(Entities.StoreDBContext dbContext) : base(dbContext) { }

        public IEnumerable<Order> GetOrders()
        {
            return _dbContext.Order
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .Include(o => o.ProductOrder)
                .ThenInclude(po => po.Product)
                .Select(Mapper.Map);
        }
        public Order GetOrderById(int id)
        {
            var order = _dbContext.Order
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .Include(o => o.ProductOrder)
                .ThenInclude(po => po.Product)
                .FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                var ex = new KeyNotFoundException($"No order with ID {id} exists in database");
                _logger.Error(ex, ex.Message);
                throw ex;
            } else
            {
                _logger.Info("Getting order with ID {id}");
                return Mapper.Map(order);
            }
        }

        public IEnumerable<Order> GetOrderHistoryByCustomer(int customerId)
        {
            return _dbContext.Order
                .Where(o => o.BuyerId == customerId)
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .Include(o => o.ProductOrder)
                .ThenInclude(po => po.Product)
                .Select(Mapper.Map);
        }

        public IEnumerable<Order> GetOrderHistoryByLocation(int locationId)
        {
            return _dbContext.Order
                .Where(o => o.SellerId == locationId)
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .Include(o => o.ProductOrder)
                .ThenInclude(po => po.Product)
                .Select(Mapper.Map);
        }

        public IEnumerable<Order> GetOrderHistoryInRange(DateTime min, DateTime max)
        {
            return _dbContext.Order
                .Where(o => o.LastModified >= min && o.LastModified <= max)
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .Include(o => o.ProductOrder)
                .ThenInclude(po => po.Product)
                .Select(Mapper.Map);
        }
        public IEnumerable<Order> GetOrdersInPriceRange(double min, double max)
        {
            return _dbContext.Order
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .Include(o => o.ProductOrder)
                .ThenInclude(po => po.Product)
                .Select(Mapper.Map)
                .Where(o => o.Total >= min && o.Total <= max);
        }

        public IEnumerable<Order> GetOrdersWithOpenState(bool isOpen)
        {
            return _dbContext.Order
                .Where(o => o.IsOpen == isOpen)
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .Include(o => o.ProductOrder)
                .ThenInclude(po => po.Product)
                .Select(Mapper.Map);
        }

        public void AddOrder(Order order)
        {
            if (order.Id != 0)
            {
                _logger.Warn($"Order with ID {order.Id} already exists in database: ignoring");
            } else
            {
                var entity = Mapper.Map(order);
                entity.Id = 0;
                _logger.Info($"Adding new order to database");
                _dbContext.Order.Add(entity);
            }
            
        }
        
        public void UpdateOrder(Order order)
        {
            if (order.Id == 0)
            {
                var ex = new KeyNotFoundException($"Order not found in database");
                _logger.Error(ex, ex.Message);
                throw ex;
            }
            var currentEntity = _dbContext.Order.Find(order.Id);
            var newEntity = Mapper.Map(order);
            var oldProductOrders = currentEntity.ProductOrder.ToList();
            var newProductOrders = newEntity.ProductOrder.ToList();
            for (int i = 0; i < order.ProductSet.Count; i++)
            {
                _logger.Info($"Updating quantity of {oldProductOrders[i].Product.Name} in order #{order.Id}");
                _dbContext.Entry(oldProductOrders[i]).CurrentValues.SetValues(newProductOrders[i]);
            }
            _logger.Info($"Updating info for order #{order.Id}");
            _dbContext.Entry(currentEntity).CurrentValues.SetValues(newEntity);
        }

        public Order GetLatestOrder(int customerId, int locationId)
        {
            var order = _dbContext.Order.OrderByDescending(o => o.LastModified)
                .Where(o => o.BuyerId == customerId && o.SellerId == locationId)
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .Include(o => o.ProductOrder)
                .ThenInclude(po => po.Product).FirstOrDefault();
            if (order == null)
            {
                var ex = new KeyNotFoundException("Customer {customer.FullName} has no orders at {location.Name} store");
                _logger.Error(ex, ex.Message);
                throw ex;
            }
            _logger.Info($"Getting latest order for customer #{customerId} at location #{locationId}");
            return Mapper.Map(order);
        }
    }
}
