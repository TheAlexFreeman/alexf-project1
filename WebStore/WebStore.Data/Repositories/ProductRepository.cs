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
    public class ProductRepository : Repository, IProductRepository
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public ProductRepository(Entities.StoreDBContext dbContext) : base(dbContext) { }
        public IEnumerable<Product> GetProducts(string search = "")
        {
            _logger.Info($"Getting products with name containing {search}");
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
                .FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                var ex = new KeyNotFoundException($"No product with ID {id} exists in database");
                _logger.Error(ex, ex.Message);
                throw ex;
            } else
            {
                _logger.Info($"Getting product with ID {id}");
                return Mapper.Map(product);
            }
        }

        public Product GetProductByName(string name)
        {
            var product = _dbContext.Product
                            .Include(p => p.ProductItem)
                            .ThenInclude(pi => pi.Item)
                            .FirstOrDefault(p => p.Name == name);
            if (product == null)
            {
                var ex = new KeyNotFoundException($"No product with name {name} exists in database");
                _logger.Error(ex, ex.Message);
                throw ex;
            }
            else
            {
                _logger.Info($"Getting product with name {name}");
                return Mapper.Map(product);
            }
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
