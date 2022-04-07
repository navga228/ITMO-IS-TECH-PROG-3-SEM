using System;
using System.Linq;

namespace Banks
{
    public class Withdraw : IOperation
    {
        private readonly IAccount _account;
        private readonly float _money;
        private bool _isComplete;

        public Withdraw(IAccount account, float money)
        {
            if (account == null)
            {
                throw new BankException("account is null!");
            }

            _account = account;
            _money = money;
            Id = Guid.NewGuid().ToString("N");
        }

        public bool IsComplete
        {
            get => _isComplete;
        }

        public string Id { get; }

        public void Execute()
        {
            if (_account.IsVerify && _account.Balance - _money < 0)
            {
                throw new BankException("Недостаточно средств");
            }

            if (!_account.IsVerify && _money > _account.BankConditions.UnverifiedLimit)
            { // Если аккаунт не верифицирован и клиент хочет снять сумму большую чем возможно для неверифицированных аккаунтов
                throw new BankException("Для снятия такой суммы нужно верифицировать аккаунт");
            }

            _account.Withdraw(_money);
            _isComplete = true;
        }

        public void CancelOperation()
        {
            if (_isComplete != true) throw new BankException("Операция, которую вы хотите отменить не была совершена");
            _account.Balance += _money;
        }
    }
}