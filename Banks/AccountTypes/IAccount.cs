using System;

namespace Banks
{
    public interface IAccount
    {
        public float Balance { get; set; }
        public bool IsVerify { get; set; }
        public string Name { get; }
        public Guid CardNumber { get; }
        public BankConditions BankConditions { get; }
        public void Withdraw(float money);
        public void Deposite(float money);
        public void Transfer(float money, IAccount account);
        public void AfterOneDay();
    }
}