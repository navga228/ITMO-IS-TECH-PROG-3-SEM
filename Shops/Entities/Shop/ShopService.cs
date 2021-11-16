using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops.Entities
{
    public class ShopService
    {
        private List<Shop> _shops = new List<Shop>();
        public Shop AddShop(string name, int cash)
        {
            Shop newShop = new Shop(name, cash);
            _shops.Add(newShop);
            return newShop;
        }

        public Shop FindCheapestBatch(Dictionary<Product, int> productListToBuy)
        {
            if (productListToBuy == null || !productListToBuy.Any())
            {
                throw new ProductException("product list to buy null or empty");
            }

            Shop ans = null;
            int minBatchPrice = int.MaxValue;
            foreach (var shop in _shops)
            {
                int batchPrice = 0;
                foreach (var product in productListToBuy)
                {
                    var com = shop.CommodityList.Where(i => i.Product.ID == product.Key.ID).FirstOrDefault();
                    int index = shop.CommodityList.IndexOf(com);
                    if (shop.CommodityList.Any(item => item.Product.ID == product.Key.ID) &&
                        shop.CommodityList[index].ProductDescription.Amount - product.Value >= 0)
                    {
                        batchPrice += shop.CommodityList[index].ProductDescription.Price * product.Value;
                    }
                    else
                    {
                        batchPrice = 0;
                        break;
                    }
                }

                if (batchPrice < minBatchPrice && batchPrice != 0)
                {
                    minBatchPrice = batchPrice;
                    ans = shop;
                }
            }

            if (ans != null)
            {
                return ans;
            }

            return null;
        }
    }
}