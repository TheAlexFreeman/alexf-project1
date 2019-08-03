using System;
using System.Collections.Generic;
using System.Text;
using WebStore.BLL;
using WebStore.BLL.Interfaces;

namespace WebStore.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public Product GetProductById(int id)
        {
            throw new NotImplementedException();
        }

        public Product GetProductByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetProducts(string search = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetProductsInPriceRange(double min, double max)
        {
            throw new NotImplementedException();
        }
    }
}
