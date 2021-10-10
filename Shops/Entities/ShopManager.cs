using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security;
using Shops.Tools;

namespace Shops.Entities
{
    public class ShopManager
    {
        private List<Product> _poducts = new List<Product>();
        private List<Shop> _shops = new List<Shop>();

        public Shop AddShop(string name, long cash)
        {
            Shop newShop = new Shop(name, cash);
            _shops.Add(newShop);
            return newShop;
        }

        public Product RegisterProduct(string name) // Добавление продукта в систему
        {
            string nameLower = name.ToLower();
            foreach (Product i in _poducts)
            {
                if (i.ProductName == nameLower)
                {
                    throw new ShopsExceptions("Such products alredy exists");
                }
            }

            Product newProduct = new Product(nameLower);
            return newProduct;
        }

        public Shop SerchBatch(Dictionary<Product, int> products)
        {
            Shop ans = null;
            int minBatchPrice = int.MaxValue;
            int priceCounter = 0;
            bool flag = false; // Флаг того что хоть один магазин содержащий партию найден

            // bool flag1 = false; // Нйден магазин где есть весь товар, но недостаточно по кол-ву
            foreach (Shop i in _shops)
            {
                bool flag2 = false; // Флаг того что какого-то товара в магазине уже нет и чтоб лишний раз не перебирать
                foreach (var j in products)
                {// Проверяем каждый товар на наличие
                    if (i.ProductsList.ContainsKey(j.Key) && i.ProductsList[j.Key].Amount >= j.Value)
                    {
                        priceCounter = priceCounter + (i.ProductsList[j.Key].Price * j.Value);
                    }
                    else
                    {
                        flag2 = true;
                    }
                }

                if (!flag2 && priceCounter < minBatchPrice)
                {
                    ans = i;
                    flag = true;
                }
            }

            // Если магазина содержащего партию нет
            if (!flag)
            {
                throw new ShopsExceptions("No shop containing the batch");
            }

            return ans;
        }
    }
}