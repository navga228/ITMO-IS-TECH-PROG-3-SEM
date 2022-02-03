using System;

namespace Banks
{
    public interface IAccount
    {
        public Client AccountOwner { get; }
        public float Balance { get; set; }
        public string Name { get; }
        public Guid CardNumber { get; }
        public void Withdraw(float money);
        public void Deposite(float money);
        public void Transfer(float money, IAccount account);
        public void AfterOneDay();
    }
}