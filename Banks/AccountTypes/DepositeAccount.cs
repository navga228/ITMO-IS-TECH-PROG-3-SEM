using System;
using System.Collections.Generic;
using System.Linq;

namespace Banks
{
    public class DepositeAccount : IAccount
    { // Сделать в интрфейсе аккаунтов общие методы вместо прямого доступа к полям
        private int _daysToWithdraw; // Дни после которых можно выводить деньги со счета
        private int _days;
        private float _moneyFromInterest;
        public DepositeAccount(BankConditions bankConditions, bool isVerify)
        {
            if (bankConditions == null)
            {
                throw new BankException("bankConditions is null!");
            }

            _daysToWithdraw = bankConditions.TimeToWithdrawOnDepositeAccount;
            BankConditions = bankConditions;
            _days = 0;
            IsVerify = isVerify;
        }

        public bool IsVerify { get; set; }
        public BankConditions BankConditions { get; }
        public string Name { get; } = "DepositeCard";
        public Guid CardNumber { get; } = Guid.NewGuid();
        public float Balance { get; set; }

        public void Withdraw(float money)
        {
            if (_daysToWithdraw > 0) return;
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
            if (BankConditions.TimeToWithdrawOnDepositeAccount < 0) return;
            var procentToday = BankConditions.BankInterest.Where(inter => inter.Sum[0] >= Balance && inter.Sum[1] <= Balance)
                .Select(inter => inter.Percent).FirstOrDefault();
            _moneyFromInterest += Balance * (procentToday / 365);
            _daysToWithdraw--;
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