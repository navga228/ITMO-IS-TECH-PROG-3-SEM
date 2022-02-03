using System;

namespace Banks
{
    public class Deposite : IOperation
    {
        private readonly IAccount _account;
        private readonly float _money;
        private bool _isComplete;

        public Deposite(IAccount account, float money)
        {
            _account = account;
            _money = money;
            _isComplete = false;
            Id = Guid.NewGuid().ToString("N");
        }

        public bool IsComplete
        {
            get => _isComplete;
        }

        public string Id { get; }

        public void Execute()
        { // Выполнение операции
            _account.Deposite(_money);
            _isComplete = true;
        }

        public void CancelOperation()
        {
            if (_isComplete != true) throw new BankException("Операция не была совершена");
            _account.Balance -= _money;
        }
    }
}