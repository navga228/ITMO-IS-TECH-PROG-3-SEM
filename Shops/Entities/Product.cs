namespace Shops.Entities
{
    public class Product
    {
        private static int _nextProductId;

        public Product(string name)
        {
            Id = _nextProductId;
            _nextProductId++;
            ProductName = name;
        }

        public Product(string name, int id)
        {
            ProductName = name;
            Id = id;
        }

        public string ProductName { get; }

        public int Id { get; }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}