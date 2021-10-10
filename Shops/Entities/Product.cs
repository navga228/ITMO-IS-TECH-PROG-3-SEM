namespace Shops.Entities
{
    public class Product
    {
        private static int _nextProductID;

        public Product()
        {
            ID = 0;
            ProductName = null;
        }

        public Product(string name)
        {
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

        public override int GetHashCode()
        {
            return ID;
        }
    }
}