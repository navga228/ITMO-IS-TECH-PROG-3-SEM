using System;

namespace Banks
{
    public class CreditAccount : IAccount
    {
        private float _creditLimit; // Кредитный лимит
        private float _comission; // Комиссия за пользование кредитными деньгами
        private int _days;
        private float _moneyFromInterest;
        private float _interestOnBalance;

        public CreditAccount(Client client, float creditLimit, float comission, float interestOnBalance)
        {
            AccountOwner = client;
            _creditLimit = creditLimit;
            _comission = comission;
            _days = 0;
            _interestOnBalance = interestOnBalance;
        }

        public string Name { get; } = "CreditCard";
        public Guid CardNumber { get; } = Guid.NewGuid();

        public Client AccountOwner { get; }
        public float Balance { get; set; }
        public void Withdraw(float money)
        {
            if (Balance - money < Balance - _creditLimit)
            {
                throw new BankException("Сумма которую вы хотите вывести превышает ваш кредитный лимит");
            }

            Balance -= money;
        }

        public void Deposite(float money)
        {
            Balance += money;
        }

        public void Transfer(float money, IAccount account)
        {
            if (_creditLimit > Balance - money)
            {
                throw new BankException("Сумма которую вы хотите перевести превышает ваш кредитный лимит");
            }

            Balance -= money;
            account.Balance += money;
        }

        public void AfterOneDay()
        {
            _days++;
            if (Balance > 0)
            {
                _moneyFromInterest += Balance * (_interestOnBalance / 365);
            }

            if (Balance < 0)
            {
                _moneyFromInterest -= _comission;
            }

            if (_days != 30) return;
            _days = 0;
            AfterOneMonth();
        }

        public void AfterOneMonth()
        {
            Balance += _moneyFromInterest;
            _moneyFromInterest = 0;
        }
    }
}