using System;
using System.Collections.Generic;
using System.Text;
using WebStore.BLL;
using WebStore.BLL.Interfaces;

namespace WebStore.Data.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        public Location GetLocationById(int id)
        {
            throw new NotImplementedException();
        }

        public Location SearchLocationByName(string search)
        {
            throw new NotImplementedException();
        }

        public Location SearchLocationByProductName(string search)
        {
            throw new NotImplementedException();
        }
    }
}
