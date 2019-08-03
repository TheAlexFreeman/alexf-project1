using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.BLL.Interfaces
{
    public interface ILocationRepository
    {
        Location GetLocationById(int id);
        Location SearchLocationByName(string search);
        Location SearchLocationByProductName(string search);
    }
}
