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
        public ItemRepository(Project0DBContext dbContext) : base(dbContext) { }
        public void AddItem(BLL.Item item)
        {
            throw new NotImplementedException();
        }

        public void DeleteItem(int id)
        {
            throw new NotImplementedException();
        }

        public void EditItem(int id, BLL.Item item)
        {
            throw new NotImplementedException();
        }

        public BLL.Item GetItemById(int id)
        {
            //throw new NotImplementedException();
            return _dbContext.Item.FirstOrDefault(i => i.Id == id);
        }
        public BLL.Item GetItemByName(string name)
        {
            //throw new NotImplementedException();
            return _dbContext.Items.FirstOrDefault(i => i.Name == name);
        }

        public IEnumerable<Item> GetItems(string search = "")
        {
            //return _dbContext.Items.Where(i => i.Name.Contains(search));
            throw new NotImplementedException();
        }

        public IEnumerable<BLL.Item> GetItemsInCostRange(double min, double max)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BLL.Item> SearchItemsByName(string search)
        {
            throw new NotImplementedException();
        }

        BLL.Item IItemRepository.GetItemById(int id)
        {
            throw new NotImplementedException();
        }

        BLL.Item IItemRepository.GetItemByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
