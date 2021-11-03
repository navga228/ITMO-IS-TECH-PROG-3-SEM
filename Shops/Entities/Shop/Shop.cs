using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using Shops.Entities.Delivery;
using Shops.Tools;

namespace Shops.Entities
{
    public class Shop
    {
        private static uint _nextShopID;
        private uint _shopID;

        public Shop(string name, long cash)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new SupplierException("Name of shop is null or empty");
            }

            CommodityList = new List<Commodity>();
            BatchesOfCommodities = new List<(Supplier supplier, Dictionary<Product, ProductDescription> commodityListToDelivery)>();
            _shopID = _nextShopID;
            _nextShopID++;
            ShopName = name;
            Cashbox = cash;
        }

        public List<Commodity> CommodityList { get; } // товары которые есть в магазе
        public string ShopName { get; }
        public long Cashbox { get; set; }
        public List<(Supplier supplier, Dictionary<Product, ProductDescription> commodityListToDelivery)> BatchesOfCommodities { get; } // Учет всех поставок

        public void ChangeCommodityPrice(Commodity commodity, int newPrice)
        {
            var serchedCommodity = CommodityList.Where(item => item.Product.ID == commodity.Product.ID).FirstOrDefault();
            var index = CommodityList.IndexOf(serchedCommodity);
            CommodityList[index].ProductDescription.Price = newPrice;
        }
    }
}