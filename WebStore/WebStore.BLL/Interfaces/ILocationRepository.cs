using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.BLL.Interfaces
{
    public interface ILocationRepository
    {
        IEnumerable<Location> GetLocations();
        Location GetLocationById(int id);
        Location GetLocationByName(string name);
        //Location SearchLocationByName(string search);
        //Location SearchLocationByProductName(string search);
    }
}
