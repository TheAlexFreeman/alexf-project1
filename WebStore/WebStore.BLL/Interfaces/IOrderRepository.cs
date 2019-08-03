using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.BLL.Interfaces
{
    public interface IOrderRepository
    {
        Order GetOrderById(int id);
        IEnumerable<Order> GetOrderHistoryByLocation(int locationId);
        IEnumerable<Order> GetOrderHistoryByCustomer(int customerId);
        IEnumerable<Order> GetOrderHistoryInRange(DateTime min, DateTime max);
        IEnumerable<Order> GetOrdersInPriceRange(double min, double max);
    }
}
