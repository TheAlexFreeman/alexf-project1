using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.BLL;
using WebStore.BLL.Interfaces;
using WebStore.Data.Entities;

namespace WebStore.Data.Repositories
{
    public class ItemRepository : Repository, IItemRepository
    {
        //private readonly WebStoreContext _dbContext;
        //public ItemRepository(WebStoreContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}
        public ItemRepository(StoreDBContext dbContext) : base(dbContext) { }
        public void AddItem(BLL.Item item)
        {
            if (item.Id != 0)
            {
                //_logger.Warn($"Item to be added has an ID ({item.Id}) already: ignoring.");
                throw new ArgumentException($"Cannot add existing Item with ID {item.Id} to database");
            }
            //_logger.Info($"Adding item");
            Entities.Item entity = Mapper.Map(item);
            _dbContext.Add(entity);
            item.Id = _dbContext.Item.Count();
        }

        public void DeleteItem(int id)
        {
            var toDelete = _dbContext.Item.Find(id);
            if (toDelete == null)
            {
                throw new ArgumentException($"Item with ID {id} does not exist in database");
            }
            _dbContext.Item.Remove(toDelete);
        }

        public void EditItem(int id, BLL.Item item)
        {
            var toEdit = _dbContext.Item.Find(id);
            if (toEdit == null)
            {
                throw new ArgumentException($"Item with ID {id} does not exist in database");
            }
            item.Id = toEdit.Id;
            var newItem = Mapper.Map(item);
            _dbContext.Entry(toEdit).CurrentValues.SetValues(newItem);
        }

        public BLL.Item GetItemById(int id)
        {
            //throw new NotImplementedException();
            return Mapper.Map(_dbContext.Item.FirstOrDefault(i => i.Id == id));
        }
        public BLL.Item GetItemByName(string name)
        {
            //throw new NotImplementedException();
            return Mapper.Map(_dbContext.Item.FirstOrDefault(i => i.Name == name));
        }

        public IEnumerable<BLL.Item> SearchItemsByName(string search = "")
        {
            return _dbContext.Item.Where(i => i.Name.Contains(search)).Select(Mapper.Map);
            //throw new NotImplementedException();
        }

        public IEnumerable<BLL.Item> GetItemsInCostRange(double min, double max)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        //public IEnumerable<BLL.Item> SearchItemsByName(string search)
        //{
        //    throw new NotImplementedException();
        //}

        //BLL.Item IItemRepository.GetItemById(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //BLL.Item IItemRepository.GetItemByName(string name)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
