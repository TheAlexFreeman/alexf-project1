using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.BLL;

namespace WebStore.Data.Repositories
{
    public class ItemRepository
    {
        private readonly WebStoreContext _dbContext;
        public ItemRepository(WebStoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Item GetItemById(int id)
        {
            return _dbContext.Items.FirstOrDefault(i => i.Id == id);
        }
        public Item GetItemByName(string name)
        {
            return _dbContext.Items.FirstOrDefault(i => i.Name == name);
        }

        public IEnumerable<Item> GetItems(string search = "")
        {
            return _dbContext.Items.Where(i => i.Name.Contains(search));
        }
    }
}
