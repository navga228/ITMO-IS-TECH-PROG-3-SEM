using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using Shops.Entities;
using Shops.Tools;

namespace Shops.Entities
{
    public class Shop
    {
        private static uint _nextShopID;
        private uint _shopID;
        private int _cashbox;

        public Shop(string name, int cash)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new SupplierException("Name of shop is null or empty");
            }

            CommodityList = new List<Commodity>();
            BatchesOfCommodities = new List<Batches>();
            _shopID = _nextShopID;
            _nextShopID++;
            ShopName = name;
            _cashbox = cash;
        }

        public List<Commodity> CommodityList { get; } // товары которые есть в магазе
        public string ShopName { get; }
        public List<Batches> BatchesOfCommodities { get; set; }// Учет всех поставок

        public void ChangeCommodityPrice(Commodity commodity, int newPrice)
        {
            var serchedCommodity = CommodityList.Where(item => item.Product.ID == commodity.Product.ID).FirstOrDefault();
            var index = CommodityList.IndexOf(serchedCommodity);
            CommodityList[index].ProductDescription.Price = newPrice;
        }

        public void AddMoneyInCashbox(int sumToAdd)
        {
            _cashbox += sumToAdd;
        }

        public void SubstractMoneyFromCashbox(int sumToSubstract)
        {
            _cashbox -= sumToSubstract;
        }
    }
}