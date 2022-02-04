using System;
using System.Collections.Generic;

namespace Banks
{
    public class CentralBank
    {
        private OperationManager _operationManager = new OperationManager();
        public CentralBank(string name)
        {
            Name = name;
        }

        public Guid Id { get; } = Guid.NewGuid();
        public List<Bank> Banks { get; } = new List<Bank>();
        private string Name { get; }
        public Bank AddNewBank(string name, List<Interest> bankInterest, float unverifiedLimit, float creditLimit, float comission, float interestOnDebitBalance, float interestOnCreditBalance, int timeToWithdrawOnDepositeAccount)
        {
            Bank bank = new Bank(name, bankInterest, unverifiedLimit, creditLimit, comission, interestOnDebitBalance, interestOnCreditBalance, timeToWithdrawOnDepositeAccount);
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
    }
}