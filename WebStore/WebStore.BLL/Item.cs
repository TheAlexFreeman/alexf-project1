using System;

namespace WebStore.BLL
{
    /// <summary>
    /// An item in some store's inventory
    /// </summary>
    public class Item
    {
        public Item(string name, double cost, int id = 0)
        {
            Id = id;
            Name = name;
            Cost = cost;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
    }
}
