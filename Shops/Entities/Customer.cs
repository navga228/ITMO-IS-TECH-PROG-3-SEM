namespace Shops.Entities
{
    public class Customer
    {
        private string _name;
        public Customer(string name, int money)
        {
            Money = money;
            _name = name;
        }

        public int Money { get; set; }
    }
}