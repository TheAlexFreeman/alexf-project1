﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.BLL.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts(string search);
        Product GetProductById(int id);
        Product GetProductByName(string name);
        IEnumerable<Product> GetProductsInPriceRange(double min, double max);
        void Save();

    }
}
