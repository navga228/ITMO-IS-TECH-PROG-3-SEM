using System;
using System.Data;

namespace Banks
{
    public class DebitAccount : IAccount
    {
        private int _days;
        private float _moneyFromInterest;
        public DebitAccount(BankConditions bankConditions, bool isVerify)
        {
            if (bankConditions == null)
            {
                throw new BankException("bankConditions is null!");
            }

            BankConditions = bankConditions;
            _days = 0;
            IsVerify = isVerify;
        }

        public bool IsVerify { get; set; }
        public BankConditions BankConditions { get; }
        public string Name { get; } = "DebitCard";
        public Guid CardNumber { get; } = Guid.NewGuid();
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
            if (account == null)
            {
                throw new BankException("account is null!");
            }

            Balance -= money;
            account.Balance += money;
        }

        public void AfterOneDay()
        {
            _days++;
            _moneyFromInterest += Balance * (BankConditions.InterestOnDebitBalance / 365);
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