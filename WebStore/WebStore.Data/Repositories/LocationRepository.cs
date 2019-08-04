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
        private readonly DbSet<Entities.Location> _locations;
        public LocationRepository(Entities.StoreDBContext dbContext) : base(dbContext)
        {
            _locations = dbContext.Location;
        }
        public IEnumerable<Location> GetLocations()
        {
            return _locations.Select(Mapper.Map);
        }
        public Location GetLocationById(int id)
        {
            var location = _locations
                .Include(l => l.ProductLocation)
                .ThenInclude(pl => pl.Product)
                .Include(l => l.InventoryItem)
                .ThenInclude(ii => ii.Item)
                .FirstOrDefault(l => l.Id == id) ?? throw new KeyNotFoundException($"No location with ID {id} exists in database");
            return Mapper.Map(location);
        }
        public Location GetLocationByName(string name)
        {
            var location = _locations
               .Include(l => l.ProductLocation)
               .ThenInclude(pl => pl.Product)
               .Include(l => l.InventoryItem)
               .ThenInclude(ii => ii.Item)
               .FirstOrDefault(l => l.Name == name) ?? throw new KeyNotFoundException($"No location with name '{name}' exists in database");
            return Mapper.Map(location);
        }


        public void DeleteLocation(int id)
        {
            if (_locations.Any(l => l.Id == id))
            {
                _locations.Remove(_locations.Find(id));
            } else
            {
                throw new KeyNotFoundException("No location with ID {id} exists in database");
            }
        }

        public void EditLocation(int id, Location newLocation)
        {
            if (_locations.Any(l => l.Id == id))
            {
                var currentEntity = _locations.Find(id);
                var newEntity = Mapper.Map(newLocation);
                _dbContext.Entry(currentEntity).CurrentValues.SetValues(newEntity);
            }
            else
            {
                throw new KeyNotFoundException("No location with ID {id} exists in database");
            }
        }

        public void AddLocation(Location newLocation)
        {
            if (newLocation == null)
            {
                throw new ArgumentNullException("Cannot add null location to database");
            }
            if (_locations.Any(l => l.Id == newLocation.Id))
            {
                throw new KeyNotFoundException("No location with ID {id} exists in database");
            }
            _locations.Add(Mapper.Map(newLocation));
        }
    }
}
