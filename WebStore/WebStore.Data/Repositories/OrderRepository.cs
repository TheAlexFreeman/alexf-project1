using System;
using System.Collections.Generic;
using System.Text;
using WebStore.BLL;
using WebStore.BLL.Interfaces;

namespace WebStore.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public Order GetOrderById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetOrderHistoryByCustomer(int customerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetOrderHistoryByLocation(int locationId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetOrderHistoryInRange(DateTime min, DateTime max)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetOrdersInPriceRange(double min, double max)
        {
            throw new NotImplementedException();
        }
    }
}
