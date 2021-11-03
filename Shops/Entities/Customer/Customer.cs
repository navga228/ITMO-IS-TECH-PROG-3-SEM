using Shops.Tools;

namespace Shops.Entities
{
    public class Customer
    {
        private string _name;
        public Customer(string name, int money)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new CustomerException("Name of customer is null or empty");
            }

            Money = money;
            _name = name;
        }

        public int Money { get; set; }
    }
}