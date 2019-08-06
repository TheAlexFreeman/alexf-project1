using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.BLL.Interfaces
{
    public interface ILocationRepository
    {
        IEnumerable<Location> GetLocations(string search);
        Location GetLocationById(int id);
        Location GetLocationByName(string name);
        void Save();
        void DeleteLocation(int id);
        void EditLocation(int id, Location newLocation);
        void AddLocation(Location newLocation);
        IEnumerable<Order> GetOrderHistory(int id);
        
    }
}
