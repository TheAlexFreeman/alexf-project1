using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.BLL.Interfaces
{
    public interface IItemRepository
    {
        Item GetItemById(int id);
        Item GetItemByName(string name);
        IEnumerable<Item> SearchItemsByName(string search);
        IEnumerable<Item> GetItemsInCostRange(double min, double max);

        void AddItem(Item item);
        void DeleteItem(int id);
        void EditItem(int id, Item item);

        void Save();
    }
}
