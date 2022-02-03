using System;
using System.Data;

namespace Banks
{
    public class DebitAccount : IAccount
    {
        private float _interestOnBalance; // Проценты на остаток
        private int _days;
        private float _moneyFromInterest;
        public DebitAccount(Client accountOwner, float interestOnDebitBalance)
        {
            AccountOwner = accountOwner;
            _interestOnBalance = interestOnDebitBalance;
            _days = 0;
        }

        public string Name { get; } = "DebitCard";
        public Guid CardNumber { get; } = Guid.NewGuid();

        public Client AccountOwner { get; }
        public float Balance { get; set; }

        public void Withdraw(float money)
        {
            Balance -= money;
        }

        public void Deposite(float money)
        {
            Balance += money;
        }

        public void Transfer(float money, IAccount account)
        {
            Balance -= money;
            account.Balance += money;
        }

        public void AfterOneDay()
        {
            _days++;
            _moneyFromInterest += Balance * (_interestOnBalance / 365);
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