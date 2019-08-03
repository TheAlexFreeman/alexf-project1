using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.BLL.Interfaces
{
    interface IProductRepository
    {
        Product GetProductById(int id);
        Product GetProductByName(string name);
        IEnumerable<Product> GetProducts(string search = null);
        IEnumerable<Product> GetProductsInPriceRange(double min, double max);
    }
}
