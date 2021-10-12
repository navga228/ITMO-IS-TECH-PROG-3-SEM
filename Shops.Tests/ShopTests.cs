using System.Collections.Generic;
using System.Linq;
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
            
            Assert.AreEqual(moneyBefore - productPrice * productToBuyAmount, customer.Money);
            Assert.AreEqual(productAmount - productToBuyAmount, newShop.ProductsList.ElementAt(0).Value.Amount);
        }

        [Test]
        [TestCase(10, 20)]
        public void ChangeProductPriceAndPriceChanged(int oldPrice, int newPrice)
        {
            var newShop = _shopManager.AddShop("Дикси", 100000);
            var product = _shopManager.RegisterProduct("Батон");
            
            // Создаем список продуктов доставляемых в магазин с описанием цены и количества 
            var productListToShop = new Dictionary<Product, ProductDescription>();
            productListToShop.Add(product, new ProductDescription(10, oldPrice));
            
            //Поставляем эти продукты в магаз
            newShop.AddProducts(productListToShop);
            
            // Меняем цену
            newShop.ChangePrice(product, newPrice);
            Assert.AreEqual(newPrice, newShop.ProductsList.ElementAt(0).Value.Price);
        }

        [Test]
        public void BuyCheapestBatchOfProducts_CheapestBatchBought()
        {// Создать 3 магазина; Тот который подходит, и 3 которые не подходят; Обрабоать ситуацию что товара может быть недостаточно и товара мо
            // может быть нигде;
            // Ситуация 1: Во всех магазинах есть товары нужные но в одном дешевле
            // Ситуация 2: Ни в каком магазине нет нужной партии
            // Ситуация 3: Во всех магазинах есть продукты из партии, но нигде не хватает их кол-ва
            
            
            //Создаем магазины
            Shop magnitExpect = _shopManager.AddShop("Магнит", 100000);
            Shop piatorochka = _shopManager.AddShop("Пятерочка", 100000);
            Shop diksy = _shopManager.AddShop("Дикси", 100000);
            
            //Заполняем магазины продуктами
            Product banan = new Product("Банан");
            Product snikers = new Product("Сникерс");
            Product apple = new Product("Яблоко");
            var productListToShopExpect = new Dictionary<Product, ProductDescription>();
            productListToShopExpect.Add(banan, new ProductDescription(10,20));
            productListToShopExpect.Add(snikers, new ProductDescription(50,40));
            productListToShopExpect.Add(apple, new ProductDescription(100,150));
            magnitExpect.AddProducts(productListToShopExpect);
            var productListToShop1 = new Dictionary<Product, ProductDescription>();
            productListToShop1.Add(banan, new ProductDescription(10,30));
            productListToShop1.Add(snikers, new ProductDescription(50,50));
            productListToShop1.Add(apple, new ProductDescription(100,100));
            piatorochka.AddProducts(productListToShop1);
            var productListToShop2 = new Dictionary<Product, ProductDescription>();
            productListToShop2.Add(banan, new ProductDescription(10,30));
            productListToShop2.Add(snikers, new ProductDescription(50,190));
            productListToShop2.Add(apple, new ProductDescription(100,150));
            diksy.AddProducts(productListToShop2);
            
            // Партия продуктов которую ищем
            var productListToSearch = new Dictionary<Product, int>();
            productListToSearch.Add(banan, 5);
            productListToSearch.Add(snikers, 10);
            productListToSearch.Add(apple, 20);
            
            Assert.AreEqual(magnitExpect, _shopManager.SerchBatch(productListToSearch));
        }

        [Test]
        public void BuyBatchInShop_AmountChangedAndMoneyChanged()
        {
            int moneyInShopBefore = 100000;
            int moneyCustomerBefore = 4000;
            
            var customer = new Customer("Никола", moneyCustomerBefore);
            
            Shop diksy = _shopManager.AddShop("Дикси", moneyInShopBefore);
            
            Product banan = new Product("Банан");
            Product snikers = new Product("Сникерс");
            Product apple = new Product("Яблоко");

            int bananInShopBefore = 10;
            int snikersInShopBefore = 50;
            int appleInShopBefore = 100;
            
            var productListToShop = new Dictionary<Product, ProductDescription>();
            productListToShop.Add(banan, new ProductDescription(bananInShopBefore,30));
            productListToShop.Add(snikers, new ProductDescription(snikersInShopBefore,50));
            productListToShop.Add(apple, new ProductDescription(appleInShopBefore,100));
            diksy.AddProducts(productListToShop);
            
            var productListToBuy = new Dictionary<Product, int>();
            productListToBuy.Add(banan, 5); // Общая стоимость бананов 5 * 30 = 150
            productListToBuy.Add(snikers, 10);// Общая стоимость сникерсов 10 * 50 = 500
            productListToBuy.Add(apple, 20);// Общая стоимость яблок 20 * 100 = 2000

            int BatchPrice = 2650; // Общая стоимость покупок 
            
            diksy.Buy(customer, productListToBuy);
            
            Assert.AreEqual(moneyCustomerBefore - BatchPrice, customer.Money);
            Assert.AreEqual(bananInShopBefore - productListToBuy.ElementAt(0).Value, diksy.ProductsList.ElementAt(0).Value.Amount);
            Assert.AreEqual(snikersInShopBefore - productListToBuy.ElementAt(1).Value, diksy.ProductsList.ElementAt(1).Value.Amount);
            Assert.AreEqual(appleInShopBefore - productListToBuy.ElementAt(2).Value, diksy.ProductsList.ElementAt(2).Value.Amount);
        }
        
    }
}