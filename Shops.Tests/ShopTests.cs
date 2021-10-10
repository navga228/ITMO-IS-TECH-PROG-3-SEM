using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Shops.Entities;
using Shops.Tools;
using NUnit.Framework;

namespace Shops.Tests
{
    public class ShopTests
    {
        private ShopManager _shopManager;

        [SetUp]
        public void Setup()
        {
            _shopManager = new ShopManager();
        }

        [Test]
        [TestCase(1000, 40, 10, 2)]
        public void DeliveryProductsAfterDeliveryCanBuyProducts(int moneyBefore, int productPrice, int productAmount, int productToBuyAmount)
        {
            var customer = new Customer("Вацок", moneyBefore);
            var newShop = _shopManager.AddShop("Дикси", 100000);
            var product = _shopManager.RegisterProduct("Батон");
            
            // Создаем список продуктов доставляемых в магазин с описанием цены и количества 
            var productListToShop = new Dictionary<Product, ProductDescription>();
            productListToShop.Add(product, new ProductDescription(productAmount, productPrice));

            //Поставляем эти продукты в магаз
            newShop.AddProducts(productListToShop);
            
            // Создаем список того какие продукты хочет купить покупатель и в каком количестве
            var productListToBuy = new Dictionary<Product, int>();
            productListToBuy.Add(product, productToBuyAmount);
            
            // Покупаем 2 батона в магните
            newShop.Buy(customer, productListToBuy);
            
            Assert.AreEqual(moneyBefore - productPrice  * productToBuyAmount, customer.Money);
            Assert.AreEqual(productAmount - productToBuyAmount, newShop.ProductsList[product].Amount);
        }
    }
}