using System.Collections.Generic;
using Shops.Tools;

namespace Shops.Entities
{
    public class ProductService
    {
        public Product AddProduct(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ProductException("Name of product is null or empty");
            }

            Product newProduct = new Product(name);
            return newProduct;
        }
    }
}