using System;

namespace Banks
{
    public class Transfer : IOperation
    {
        private readonly IAccount _payerAccount;
        private readonly IAccount _payeeAccount;
        private readonly float _money;
        private bool _isComplete;

        public Transfer(IAccount payer, IAccount payee, float money)
        {
            if (payer == null)
            {
                throw new BankException("payer is null!");
            }

            if (payee == null)
            {
                throw new BankException("payee is null!");
            }

            _payerAccount = payer;
            _payeeAccount = payee;
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
        {
            if (_payerAccount.IsVerify)
            {// перевод возможен если акк верифицирован
                if (_payerAccount.Balance - _money < 0) return;
                _payerAccount.Transfer(_money, _payeeAccount);
                _isComplete = true;
            }
            else
            {
                throw new BankException("Перевод для неверифицированных пользователей в другие банки невозможен");
            }
        }

        public void CancelOperation()
        {
            if (_isComplete != true) throw new BankException("Операция не была совершена");
            _payerAccount.Balance += _money;
            _payeeAccount.Balance -= _money;
        }
    }
}