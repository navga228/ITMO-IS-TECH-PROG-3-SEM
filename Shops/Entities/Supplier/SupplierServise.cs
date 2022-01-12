using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Entities;
using Shops.Tools;

namespace Shops.Entities.Delivery
{
    public class SupplierServise
    {
        public Supplier AddSupplier(string name)
        {
            Supplier newSupplier = new Supplier(name);
            return newSupplier;
        }

        public void Delivery(Supplier supplier, Shop shop, Dictionary<Product, ProductDescription> commodityListToDelivery) // Доставка продуктов в магазин
        {
            if (shop == null)
            {
                throw new SupplierException("shop is null");
            }

            if (commodityListToDelivery == null || !commodityListToDelivery.Any())
            {
                throw new SupplierException("productListToDelivery is null or empty");
            }

            // Сохраняем поставку для учета
            Batches batch = new Batches();
            batch.Supplier = supplier;
            foreach (var commodity in commodityListToDelivery)
            {
                batch.DeliveredProducts.Add(commodity.Key, commodity.Value);
            }

            shop.BatchesOfCommodities.Add(batch);

            if (!shop.CommodityList.Any())
            {
                foreach (var commodity in commodityListToDelivery)
                {
                    int id = commodity.Key.ID;
                    string name = commodity.Key.ProductName;
                    int amount = commodity.Value.Amount;
                    int price = commodity.Value.Price;
                    shop.CommodityList.Add(new Commodity(new Product(name, id), new ProductDescription(amount, price)));
                }

                return;
            }

            foreach (var commodity in commodityListToDelivery)
            {
                if (shop.CommodityList.Any(item => item.Product.ID == commodity.Key.ID))
                {
                    // Если в магазине уже есть этот продукт то просто прибавляем кол-во
                    var com = shop.CommodityList.Where(item => item.Product.ID == commodity.Key.ID).FirstOrDefault();
                    int index = shop.CommodityList.IndexOf(com);
                    shop.CommodityList[index].ProductDescription.Amount += commodity.Value.Amount;
                }
                else
                {
                    // Если нет, то добавляем этот продукт
                    int id = commodity.Key.ID;
                    string name = commodity.Key.ProductName;
                    int amount = commodity.Value.Amount;
                    int price = commodity.Value.Price;
                    shop.CommodityList.Add(new Commodity(new Product(name, id), new ProductDescription(amount, price)));
                }
            }
        }
    }
}