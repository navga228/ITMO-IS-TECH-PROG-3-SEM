using System.Collections.Generic;
using Shops.Tools;

namespace Shops.Entities
{
    public class ProductService
    {
        public Product AddProduct(string name)
        {
            Product newProduct = new Product(name);
            return newProduct;
        }
    }
}