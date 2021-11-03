using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Shops.Entities;
using Shops.Tools;
using NUnit.Framework;
using Shops.Entities.Delivery;

namespace Shops.Tests
{
    public class ShopTests
    {
        private ShopService _shopService;
        private ProductService _productService;
        private SupplierServise _supplierServise;
        private CustomerServise _customerServise;
        [SetUp]
        public void Setup()
        {
            _shopService = new ShopService();
            _productService = new ProductService();
            _supplierServise = new SupplierServise();
            _customerServise = new CustomerServise();
        }

        [Test]
        [TestCase(10000, 40, 10, 2)]
        public void DeliveryProductsAfterDeliveryCanBuyProducts(int moneyBefore, int productPrice, int productAmount, int productToBuyAmount)
        {
            // Создаем магаз
            Shop newShop = new Shop("Магнит", 1000000);

            // Создаем покупателя
            Customer customer1 = _customerServise.AddCustomer("Вацок", moneyBefore);

            // Создаем товар сникерс
            Product snikers = _productService.AddProduct("Сникерс");
            ProductDescription snikersDescription = new ProductDescription(productAmount, productPrice);

            // Создаем вендера
            Supplier supplier1 = _supplierServise.AddSupplier("Вендер1");

            // Создаем список для доставки
            Dictionary<Product, ProductDescription> listToDelivery = new Dictionary<Product, ProductDescription>();
            listToDelivery.Add(snikers, snikersDescription);

            // Поставляем
            _supplierServise.Delivery(supplier1, newShop, listToDelivery);
            
            // Создаем список для покупки
            Dictionary<Product, int> listToBuy = new Dictionary<Product, int>();
            listToBuy.Add(snikers, productToBuyAmount);

            // Покупаем
            _customerServise.Buy(customer1, newShop, listToBuy);
            
            // Проверяем
            Assert.AreEqual(moneyBefore - productPrice * productToBuyAmount, customer1.Money);
            Assert.AreEqual(productAmount - productToBuyAmount, newShop.CommodityList.ElementAt(0).ProductDescription.Amount);
        }

        [Test]
        [TestCase(10, 20)]
        public void ChangeProductPriceAndPriceChanged(int oldPrice, int newPrice)
        {
            // Создаем поставщика
            Supplier supplier1 = _supplierServise.AddSupplier("Вендер1");
            
            //Создаем магазин
            var newShop = _shopService.AddShop("Дикси", 100000);

            // Создаем список продуктов доставляемых в магазин
            var productListToShop = new Dictionary<Product, ProductDescription>();
            var product = _productService.AddProduct("Батон");
            var productDescription = new ProductDescription(10, oldPrice);
            productListToShop.Add(product, productDescription);
            
            //Поставляем эти продукты в магаз
            _supplierServise.Delivery(supplier1, newShop, productListToShop);
            
            // Меняем цену
            newShop.ChangeCommodityPrice(newShop.CommodityList[0], newPrice);
            
            Assert.AreEqual(newPrice, newShop.CommodityList.ElementAt(0).ProductDescription.Price);
        }

        [Test]
        public void SearchShopWithCheapestBatch_ShopWithCheapestBatchSearched()
        {
            Supplier supplier1 = _supplierServise.AddSupplier("Вендер1");
            //Создаем магазины
            Shop magnitExpect = _shopService.AddShop("Магнит", 100000);
            Shop piatorochka = _shopService.AddShop("Пятерочка", 100000);
            Shop diksy = _shopService.AddShop("Дикси", 100000);
            
            //Заполняем магазины продуктами
            Product banan = new Product("Банан");
            Product snikers = new Product("Сникерс");
            Product apple = new Product("Яблоко");

            // Поставка в магазин который ожидается
            Dictionary<Product, ProductDescription> productListToShopEcspect = new Dictionary<Product, ProductDescription>();
            productListToShopEcspect.Add(snikers, new ProductDescription(100, 25));
            productListToShopEcspect.Add(banan, new ProductDescription(200, 10));
            productListToShopEcspect.Add(apple, new ProductDescription(50, 40));
            _supplierServise.Delivery(supplier1, magnitExpect, productListToShopEcspect);

            // Поставка продуктов во второй магазин
            var productListToPiatorochka = new Dictionary<Product, ProductDescription>();
            productListToPiatorochka.Add(snikers, new ProductDescription(100,20));
            productListToPiatorochka.Add(banan, new ProductDescription(200,30));
            productListToPiatorochka.Add(apple, new ProductDescription(50,100));
            _supplierServise.Delivery(supplier1, piatorochka, productListToPiatorochka);
            
            // Поставка продуктов в третий магазин
            var productListToDiksy = new Dictionary<Product, ProductDescription>();
            productListToDiksy.Add(snikers, new ProductDescription(100,40));
            productListToDiksy.Add(banan, new ProductDescription(200,10));
            productListToDiksy.Add(apple, new ProductDescription(50,40));
            _supplierServise.Delivery(supplier1, diksy, productListToDiksy);

            // Партия продуктов которую ищем
            var productListToSearch = new Dictionary<Product, int>();
            productListToSearch.Add(banan, 5);
            productListToSearch.Add(snikers, 10);
            productListToSearch.Add(apple, 20);
            
            Assert.AreEqual(magnitExpect, _shopService.SerchCheapestBatch(productListToSearch));
            
            
            // Ситуация когда когда товара может быть недостаточно или не быть нигде
            var productListToSearch2 = new Dictionary<Product, int>();
            productListToSearch2.Add(banan, 10000000);
            productListToSearch2.Add(snikers, 10000);
            productListToSearch2.Add(apple, 100000);

            Shop shopExpected = null;
            
            Assert.AreEqual(shopExpected, _shopService.SerchCheapestBatch(productListToSearch2));
        }

        [Test]
        [TestCase(1000000, 10000)]
        public void BuyBatchInShop_AmountChangedAndMoneyChanged(int moneyInShopBefore, int moneyCustomerBefore)
        {
            Supplier supplier1 = _supplierServise.AddSupplier("Вендер1");
            var customer = _customerServise.AddCustomer("Никола", moneyCustomerBefore);
            
            Shop diksy = _shopService.AddShop("Дикси", moneyInShopBefore);

            int snikersInShopBefore = 50;
            int bananInShopBefore = 10;
            int appleInShopBefore = 20;

            // Создаем товар сникерс
            Product snikers = _productService.AddProduct("Сникерс");
            ProductDescription snikersDescription = new ProductDescription(snikersInShopBefore, 50);

            // Создаем товар банан
            Product banan = _productService.AddProduct("Банан");
            ProductDescription bananDescription = new ProductDescription(bananInShopBefore, 20);

            // Создаем товар яблоко
            Product apple = _productService.AddProduct("Яблоко");
            ProductDescription appleDescription = new ProductDescription(appleInShopBefore, 100);

            // Создаем список для доставки
            Dictionary<Product, ProductDescription> listToDelivery = new Dictionary<Product, ProductDescription>();
            listToDelivery.Add(snikers, snikersDescription);
            listToDelivery.Add(banan, bananDescription);
            listToDelivery.Add(apple, appleDescription);
            
            // Поставляем
            _supplierServise.Delivery(supplier1, diksy, listToDelivery);
            
            var productListToBuy = new Dictionary<Product, int>();
            productListToBuy.Add(snikers, 10);// Общая стоимость сникерсов 10 * 50 = 500
            productListToBuy.Add(banan, 5); // Общая стоимость бананов 5 * 20 = 100
            productListToBuy.Add(apple, 20);// Общая стоимость яблок 20 * 100 = 2000

            int BatchPrice = 2600; // Общая стоимость покупок 
            
            _customerServise.Buy(customer, diksy, productListToBuy);
            
            Assert.AreEqual(moneyCustomerBefore - BatchPrice, customer.Money);
            Assert.AreEqual(snikersInShopBefore - productListToBuy.ElementAt(0).Value, diksy.CommodityList.ElementAt(0).ProductDescription.Amount);
            Assert.AreEqual(bananInShopBefore - productListToBuy.ElementAt(1).Value, diksy.CommodityList.ElementAt(1).ProductDescription.Amount);
            Assert.AreEqual(appleInShopBefore - productListToBuy.ElementAt(2).Value, diksy.CommodityList.ElementAt(2).ProductDescription.Amount);
        }
        
    }
}