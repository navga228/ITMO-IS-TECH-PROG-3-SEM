using System;
using Shops.Entities.Delivery;
using Shops.Tools;

namespace Shops.Entities
{
    public class Commodity
    {// Товар который уже в самом магазине с ценой и количеством по сути обертка. Продукт с описанием попавший в магзин становится товаром
        public Commodity(Product product, ProductDescription productDescription)
        {
            if (product == null)
            {
                throw new CommodityException("product is null");
            }

            if (productDescription == null)
            {
                throw new CommodityException("productDescription is null");
            }

            Product = product;
            ProductDescription = productDescription;
        }

        public Product Product { get; }
        public ProductDescription ProductDescription { get; }
    }
}