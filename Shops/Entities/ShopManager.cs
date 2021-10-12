using System;
using System.Collections.Generic;
using System.Linq;
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
            for (int shop = 0; shop < _shops.Count; shop++)
            {
                for (int i = 0; i < products.Count; i++)
                {
                    for (int j = 0; j < _shops.ElementAt(shop).ProductsList.Count; j++)
                    {
                        if (_shops.ElementAt(shop).ProductsList.ElementAt(j).Key.ID == products.ElementAt(i).Key.ID && _shops.ElementAt(shop).ProductsList.ElementAt(j).Value.Amount >= products.ElementAt(i).Value)
                        {// Проверяем каждый товар на наличие
                            priceCounter = priceCounter + (_shops.ElementAt(shop).ProductsList.ElementAt(j).Value.Price * products.ElementAt(i).Value);
                        }
                    }
                }

                if (priceCounter < minBatchPrice)
                {
                    minBatchPrice = priceCounter;
                    ans = _shops.ElementAt(shop);
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