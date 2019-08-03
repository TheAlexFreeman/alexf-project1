using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.BLL;
using WebStore.BLL.Interfaces;

namespace WebStore.Data.Repositories
{
    public class ProductRepository : Repository, IProductRepository
    {
        public ProductRepository(Entities.StoreDBContext dbContext) : base(dbContext) { }
        public IEnumerable<Product> GetProducts()
        {
            return _dbContext.Product
                .Include(p => p.ProductItem)
                .ThenInclude(pi => pi.Item)
                .Select(Mapper.Map);
        }
        public Product GetProductById(int id)
        {
            var product = _dbContext.Product
                .Include(p => p.ProductItem)
                .ThenInclude(pi => pi.Item)
                .FirstOrDefault(p => p.Id == id) ?? throw new KeyNotFoundException($"No product with ID {id} exists in database");
            return Mapper.Map(product);
        }

        public Product GetProductByName(string name)
        {
            var product = _dbContext.Product
                            .Include(p => p.ProductItem)
                            .ThenInclude(pi => pi.Item)
                            .FirstOrDefault(p => p.Name == name)
                            ?? throw new KeyNotFoundException($"No product with name {name} exists in database");
            return Mapper.Map(product);
        }

        public IEnumerable<Product> GetProductsInPriceRange(double min, double max)
        {
            var products = _dbContext.Product
                                        .Include(p => p.ProductItem)
                                        .ThenInclude(pi => pi.Item)
                                        .Where(p => (double)p.Price >= min && (double)p.Price <= max);
            return products.Select(Mapper.Map);
        }
    }
}
