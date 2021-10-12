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
            for (int i = 0; i < ProductsList.Count; i++)
            {
                if (ProductsList.ElementAt(i).Key.ID == prod.ID)
                {
                    ProductsList.ElementAt(i).Value.Price = newPrice;
                }
            }
        }

        public void Buy(Customer person, Dictionary<Product, int> products)
        {
            for (int i = 0; i < products.Count; i++)
            {
                bool haveProduct = false;
                for (int j = 0; j < ProductsList.Count; j++)
                {
                    if (ProductsList.ElementAt(j).Key.ID == products.ElementAt(i).Key.ID)
                    {// Существует ли продукт который хочет купить покупатель в магазине
                        haveProduct = true;
                        if (person.Money - (ProductsList.ElementAt(j).Value.Price * products.ElementAt(i).Value) >= 0)
                        {// Хватает ли денег у покупателя на покупку
                            if (ProductsList.ElementAt(j).Value.Amount - products.ElementAt(i).Value >= 0)
                            {// Хватает ли продуктов в магазине
                                person.Money -= ProductsList.ElementAt(j).Value.Price * products.ElementAt(i).Value;
                                _cashbox += ProductsList.ElementAt(j).Value.Price * products.ElementAt(i).Value;
                                ProductsList.ElementAt(j).Value.Amount -= products.ElementAt(i).Value;
                            }
                            else
                            {
                                throw new ShopsExceptions("Товра который хочет купить покупатель недостаточно в магазине");
                            }
                        }
                        else
                        {
                            throw new ShopsExceptions("У покупателя недостаточно средств");
                        }
                    }
                }

                if (!haveProduct)
                {
                    throw new ShopsExceptions("Товар который хочет купить покупатель отсутствует в магазине");
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