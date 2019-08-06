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
    public class LocationRepository : Repository, ILocationRepository
    {
        private readonly DbSet<Entities.Location> _locations;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public LocationRepository(Entities.StoreDBContext dbContext) : base(dbContext)
        {
            _locations = dbContext.Location;
        }
        public IEnumerable<Location> GetLocations(string search = "")
        {
            _logger.Info($"Getting locations with name containing {search}");
            return _locations.Select(Mapper.Map);
        }
        public Location GetLocationById(int id)
        {
            var location = _locations
                .Include(l => l.ProductLocation)
                .ThenInclude(pl => pl.Product)
                .ThenInclude(p => p.ProductItem)
                .ThenInclude(pi => pi.Item)
                .Include(l => l.InventoryItem)
                .ThenInclude(ii => ii.Item)
                .FirstOrDefault(l => l.Id == id);
            if (location == null)
            {
                _logger.Error(new KeyNotFoundException(), $"No location with ID {id} exists in database");
                throw new KeyNotFoundException($"No location with ID {id} exists in database");
            }else
            {
                _logger.Info($"Getting location with ID {id} from database");
                return Mapper.Map(location);
            }            
        }
        public Location GetLocationByName(string name)
        {
            var location = _locations
               .Include(l => l.ProductLocation)
               .ThenInclude(pl => pl.Product)
               .ThenInclude(p => p.ProductItem)
               .ThenInclude(pi => pi.Item)
               .Include(l => l.InventoryItem)
               .ThenInclude(ii => ii.Item)
               .FirstOrDefault(l => l.Name == name);
            if (location == null)
            {
                _logger.Error($"No location with name {name} exists in database");
                throw new KeyNotFoundException($"No location with name {name} exists in database");
            }
            else
            {
                _logger.Info($"Getting location with name {name} from database");
                return Mapper.Map(location);
            }
        }


        public void DeleteLocation(int id)
        {
            if (_locations.Any(l => l.Id == id))
            {
                _logger.Info($"Removing location with ID {id}");
                _locations.Remove(_locations.Find(id));
            } else
            {
                var ex = new KeyNotFoundException($"No location with ID {id} exists in database");
                _logger.Error(ex, ex.Message);
                throw ex;
            }
        }

        public void EditLocation(int id, Location newLocation)
        {
            if (_locations.Any(l => l.Id == id))
            {
                newLocation.Id = id;
                var currentEntity = _locations.Find(id);
                var newEntity = Mapper.Map(newLocation);
                var oldInventory = currentEntity.InventoryItem.ToList();
                var newInventory = newEntity.InventoryItem.ToList();
                for (int i = 0; i < oldInventory.Count; i++)
                {
                    _logger.Info($"Updating quantity of {oldInventory[i].Item.Name} at {newLocation.Name} store");
                    _dbContext.Entry(oldInventory[i]).CurrentValues.SetValues(newInventory[i]);
                }
                _logger.Info($"Updating values for location with ID {newLocation.Id}");
                _dbContext.Entry(currentEntity).CurrentValues.SetValues(newEntity);
            }
            else
            {
                var ex = new KeyNotFoundException($"No location with ID {id} exists in database");
                _logger.Error(ex, ex.Message);
                throw ex;
            }
        }

        public void AddLocation(Location newLocation)
        {
            if (newLocation == null)
            {
                var argEx = new ArgumentNullException("Cannot add null location to database");
                _logger.Error(argEx, argEx.Message);
                throw argEx;
            }
            if (_locations.Any(l => l.Id == newLocation.Id))
            {
                _logger.Warn($"Location with ID {newLocation.Id} already exists in database: ignoring");
            } else
            {
                _logger.Info($"Adding new location {newLocation.Name} to database");
                _locations.Add(Mapper.Map(newLocation));
            }
            
        }

        public IEnumerable<Order> GetOrderHistory(int id)
        {
            _logger.Info($"Getting order history for location with ID {id}");
            return _dbContext.Order
                .Where(o => o.SellerId == id)
                .Include(o => o.Buyer)
                .Include(o => o.ProductOrder)
                .ThenInclude(po => po.Product)
                .ThenInclude(p => p.ProductItem)
                .ThenInclude(pi => pi.Item)
                .Select(Mapper.Map);
        }
    }
}
