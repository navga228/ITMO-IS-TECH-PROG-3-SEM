using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops.Entities
{
    public class CustomerServise
    {
        public Customer AddCustomer(string name, int money)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new CustomerException("Name of customer is null or empty");
            }

            Customer newCustomer = new Customer(name, money);
            return newCustomer;
        }

        public void Buy(Customer customer, Shop shop, Dictionary<Product, int> listToBuy)
        {
            if (shop == null)
            {
                throw new CustomerException("shop is null");
            }

            if (listToBuy == null && !listToBuy.Any())
            {
                throw new CustomerException("list to buy is null or empty");
            }

            foreach (var product in listToBuy)
            {
                if (shop.CommodityList.Any(item => item.Product.ID == product.Key.ID))
                {
                    var com = shop.CommodityList.Where(i => i.Product.ID == product.Key.ID).FirstOrDefault();
                    int index = shop.CommodityList.IndexOf(com);

                    // Достаточно ли товаров в магазине
                    if (shop.CommodityList[index].ProductDescription.Amount - product.Value < 0)
                    {
                        throw new CustomerException("Not enough commodity in the shop");
                    }

                    // Достаточно ли денег у покупателя для покупки
                    if (customer.Money - (shop.CommodityList[index].ProductDescription.Price * product.Value) < 0)
                    {
                        throw new CustomerException("The customer does not have enough money");
                    }

                    shop.Cashbox += shop.CommodityList[index].ProductDescription.Price * product.Value;
                    customer.Money -= shop.CommodityList[index].ProductDescription.Price * product.Value;
                    shop.CommodityList[index].ProductDescription.Amount -= product.Value;
                }
                else
                {
                    throw new CustomerException("No such product in the shop");
                }
            }
        }
    }
}