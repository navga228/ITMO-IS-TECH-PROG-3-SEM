using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using Shops.Tools;

namespace Shops.Entities
{
    public class Shop
    {
        private static uint _nextShopID;
        private uint _shopID;
        private long _cashbox;
        public Shop()
        {
            ProductsList = new Dictionary<Product, ProductDescription>();
            _shopID = _nextShopID;
            _nextShopID++;
            ShopName = null;
            _cashbox = 0;
        }

        public Shop(string name, long cash)
        {
            ProductsList = new Dictionary<Product, ProductDescription>();
            _shopID = _nextShopID;
            _nextShopID++;
            ShopName = name;
            _cashbox = cash;
        }

        public Dictionary<Product, ProductDescription> ProductsList { get; set; } // продукты которые есть в магазе
        public string ShopName { get; }

        public void ChangePrice(Product prod, int newPrice)
        {
            ProductsList[prod].Price = newPrice;
        }

        public void Buy(Customer person, Dictionary<Product, int> products)
        {// В словаре значениями является кол-во товара, которое хочет купить покупатель
            int customerMoneyBefore = person.Money;
            for (int i = 0; i < products.Count; i++)
            {// Если товар который хочет купить покупатель есть в магазе и его кол-во > 0
                if (!ProductsList.ContainsKey(products.ElementAt(i).Key) && ProductsList.ElementAt(i).Value.Amount <= 0)
                {
                    throw new ShopsExceptions(products.ElementAt(i).Key.ProductName + " does not exist in the shop");
                }

                if (person.Money - ProductsList.ElementAt(i).Value.Price >= 0)
                {// Если у покупаетля достаточно средств для покупки товара
                    person.Money -= ProductsList.ElementAt(i).Value.Price;
                    _cashbox += ProductsList.ElementAt(i).Value.Price;
                    ProductsList[products.ElementAt(i).Key].Amount -= products.ElementAt(i).Value;
                }
                else
                {
                    throw new ShopsExceptions("The customer does not have enough money");
                }
            }
        }

        public void AddProducts(Dictionary<Product, ProductDescription> prodList) // Поставка продуктов
        {// Если в магазине есть какие-то продукты
            if (ProductsList.Any())
            {
                for (int i = 0; i < prodList.Count; i++)
                {
                    bool flag = false;
                    for (int j = 0; j < ProductsList.Count; j++)
                    {// если такой товар уже есть в магазине
                        if (prodList.ElementAt(i).Key.ID == ProductsList.ElementAt(j).Key.ID)
                        {
                            // просто прибавляем кол-во поступившего товара к старому
                            ProductsList.ElementAt(j).Value.Amount += prodList.ElementAt(i).Value.Amount;
                            flag = true;
                        }
                    }

                    // Если же такого товара нет, то добавляем его в магаз
                    if (!flag)
                    {
                        int id = prodList.ElementAt(i).Key.ID;
                        string name = prodList.ElementAt(i).Key.ProductName;
                        int amount = prodList.ElementAt(i).Value.Amount;
                        int price = prodList.ElementAt(i).Value.Price;
                        ProductsList.Add(new Product(name, id), new ProductDescription(amount, price));
                    }
                }
            }
            else
            {// если в магазине пока нет товаров, то заполняем его новыми из списка
                for (int i = 0; i < prodList.Count; i++)
                {
                    int id = prodList.ElementAt(i).Key.ID;
                    string name = prodList.ElementAt(i).Key.ProductName;
                    int amount = prodList.ElementAt(i).Value.Amount;
                    int price = prodList.ElementAt(i).Value.Price;
                    ProductsList.Add(new Product(name, id), new ProductDescription(amount, price));
                }
            }
        }
    }
}