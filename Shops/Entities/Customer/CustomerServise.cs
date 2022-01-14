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

                    int productPrice = shop.CommodityList[index].ProductDescription.Price;
                    shop.AddMoneyInCashbox(productPrice * product.Value);
                    customer.ChangeCustomerMoney(productPrice * product.Value);
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