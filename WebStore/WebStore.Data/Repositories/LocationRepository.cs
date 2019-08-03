using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.BLL;
using WebStore.BLL.Interfaces;

namespace WebStore.Data.Repositories
{
    public class LocationRepository : Repository, ILocationRepository
    {
        public LocationRepository(Entities.StoreDBContext dbContext) : base(dbContext) { }
        public IEnumerable<Location> GetLocations()
        {
            return _dbContext.Location.Select(Mapper.Map);
        }
        public Location GetLocationById(int id)
        {
            var location = _dbContext.Location.Include(l => l.InventoryItem)
                .ThenInclude(ii => ii.Item)
                .FirstOrDefault(l => l.Id == id) ?? throw new KeyNotFoundException($"No location with ID {id} exists in database");
            return Mapper.Map(location);
        }
        public Location GetLocationByName(string name)
        {
            var location = _dbContext.Location.FirstOrDefault(l => l.Name == name) ?? throw new KeyNotFoundException($"No location with name '{name}' exists in database");
            return Mapper.Map(location);
        }
    }
}
