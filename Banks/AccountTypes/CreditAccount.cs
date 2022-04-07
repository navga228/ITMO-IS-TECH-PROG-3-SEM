using System;

namespace Banks
{
    public class CreditAccount : IAccount
    {
        private int _days;
        private float _moneyFromInterest;
        public CreditAccount(BankConditions bankConditions, bool isVerify)
        {
            if (bankConditions == null)
            {
                throw new BankException("bankConditions is null!");
            }

            _days = 0;
            BankConditions = bankConditions;
            IsVerify = isVerify;
        }

        public bool IsVerify { get; set; }
        public BankConditions BankConditions { get; }
        public string Name { get; } = "CreditCard";
        public Guid CardNumber { get; } = Guid.NewGuid();

        public float Balance { get; set; }
        public void Withdraw(float money)
        {
            if (Balance - money < Balance - BankConditions.CreditLimit)
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
            if (account == null)
            {
                throw new BankException("account is null!");
            }

            if (BankConditions.CreditLimit > Balance - money)
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
                _moneyFromInterest += Balance * (BankConditions.InterestOnCreditBalance / 365);
            }

            if (Balance < 0)
            {
                _moneyFromInterest -= BankConditions.Comission;
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