using System.Collections.Generic;

namespace Banks
{
    public class CentralBank
    {
        private OperationManager _operationManager = new OperationManager();
        public CentralBank(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public int Id { get; }
        public List<Bank> Banks { get; } = new List<Bank>();
        private string Name { get; }
        public Bank AddNewBank(string name, List<Interest> bankInterest, float unverifiedLimit, float creditLimit, float comission, float interestOnDepositBalance, float interestOnCreditBalance, int timeToWithdrawOnDepositeAccount)
        {
            Bank bank = new Bank(name, bankInterest, unverifiedLimit, creditLimit, comission, interestOnDepositBalance, interestOnCreditBalance, timeToWithdrawOnDepositeAccount);
            Banks.Add(bank);
            return bank;
        }

        public void DeleteBank(Bank bank)
        {
            if (Banks.Contains(bank))
            {
                Banks.Remove(bank);
            }
        }

        public void MakeTrasaction(IOperation operation)
        {
            _operationManager.AddOperation(operation);
            _operationManager.ProcessOperations();
        }

        public void CancelOperation(string id)
        {
            _operationManager.CancelOperation(id);
        }

        public void TimeRewindMechanism()
        {
            foreach (var bank in Banks)
            {
                foreach (var client in bank.Clients)
                {
                    foreach (var account in client.Accounts)
                    {
                        account.AfterOneDay();
                    }
                }
            }
        }
    }
}