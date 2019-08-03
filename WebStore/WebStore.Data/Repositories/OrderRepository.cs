﻿using Microsoft.EntityFrameworkCore;
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
                .FirstOrDefault(o => o.Id == id)
                ?? throw new KeyNotFoundException($"No order with ID {id} exists in database");
            return Mapper.Map(order);
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
    }
}
