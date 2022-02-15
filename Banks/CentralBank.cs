using System;
using System.Collections.Generic;

namespace Banks
{
    public class CentralBank
    {
        private OperationManager _operationManager = new OperationManager();
        public CentralBank(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new BankException("name null or empty!");
            }

            Name = name;
        }

        public Guid Id { get; } = Guid.NewGuid();
        public List<Bank> Banks { get; } = new List<Bank>();
        private string Name { get; }
        public Bank AddNewBank(string name, BankConditions bankConditions)
        {
            if (bankConditions == null)
            {
                throw new BankException("bankConditions is null!");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new BankException("name null or empty!");
            }

            Bank bank = new Bank(name, bankConditions);
            Banks.Add(bank);
            return bank;
        }

        public void DeleteBank(Bank bank)
        {
            if (bank == null)
            {
                throw new BankException("bank is null!");
            }

            if (Banks.Contains(bank))
            {
                Banks.Remove(bank);
            }
        }

        public void MakeTrasaction(IOperation operation)
        {
            if (operation == null)
            {
                throw new BankException("operation is null!");
            }

            _operationManager.AddOperation(operation);
            _operationManager.ProcessOperations();
        }

        public void CancelOperation(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new BankException("id is null or empty!");
            }

            _operationManager.CancelOperation(id);
        }
    }
}