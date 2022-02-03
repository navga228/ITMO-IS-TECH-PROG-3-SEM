using System;
using System.Collections.Generic;
using System.Linq;

namespace Banks
{
    public class DepositeAccount : IAccount
    {
        private int _daysForWithdraw;
        private List<Interest> _interest;
        private int _days;
        private float _moneyFromInterest;
        public DepositeAccount(Client client, List<Interest> interest, int daysForWithdraw)
        {
            AccountOwner = client;
            _interest = interest;
            _days = 0;
            _daysForWithdraw = daysForWithdraw;
        }

        public Client AccountOwner { get; }
        public float Balance { get; set; }
        public string Name { get; } = "DepositeCard";
        public Guid CardNumber { get; } = Guid.NewGuid();

        public void Withdraw(float money)
        {
            if (_daysForWithdraw > 0) return;
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
            if (_daysForWithdraw < 0) return;
            var procentToday = _interest.Where(inter => inter.Sum[0] >= Balance && inter.Sum[1] <= Balance)
                .Select(inter => inter.Percent).FirstOrDefault();
            _moneyFromInterest += Balance * (procentToday / 365);
            _daysForWithdraw--;
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