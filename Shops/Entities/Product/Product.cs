using Shops.Tools;

namespace Shops.Entities
{
    public class Product
    {
        private static int _nextProductID;
        public Product(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ProductException("Name of product is null or empty");
            }

            ID = _nextProductID;
            _nextProductID++;
            ProductName = name;
        }

        public Product(string name, int id)
        {
            ProductName = name;
            ID = id;
        }

        public string ProductName { get; }

        public int ID { get; }
    }
}