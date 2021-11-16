using Shops.Tools;

namespace Shops.Entities
{
    public class Customer
    {
        private string _name;
        private int _money;
        public Customer(string name, int money)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new CustomerException("Name of customer is null or empty");
            }

            _money = money;
            _name = name;
        }

        public void ChangeCustomerMoney(int sumToChange)
        {// Сделать проверку на наличие средств
            if (_money - sumToChange < 0)
            {
                throw new CustomerException("The customer does not have enough money");
            }

            _money -= sumToChange;
        }

        public int ShowBalance()
        {
            return _money;
        }
    }
}