using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Banks.Tests
{
    public class BankTests
    {
        private CentralBank _centralBank;
        private OperationManager _operationManager;
        private Bank _bank;
        [SetUp]
        public void Setup()
        {
            _centralBank = new CentralBank("Central bank");
            List<Interest> interests = new List<Interest>() { new Interest(0, 50000, 1.00f), new Interest(50000, float.MaxValue, 2.00f) };
            BankConditions bankConditions = new BankConditions(interests, 100000, 50000, 500, 1.00f, 1.00f, 0);
            _bank = _centralBank.AddNewBank("Tinkoff", bankConditions);
            
            _operationManager = new OperationManager();
        }
        
        [Test]
        [TestCase(500)]
        public void DepositeMoneyToAccountAndCreditsAddedOnBalance(float moneyToDeposite)
        {
            Client client = _bank.AddNewClient("First", "Client");

            DebitAccount debitAccount = _bank.OpenDebitAccount(client);
            CreditAccount creditAccount = _bank.OpenCreditAccount(client);
            DepositeAccount depositeAccount = _bank.OpenDepositeAccount(client);

            _operationManager.AddOperation(new Deposite(debitAccount, moneyToDeposite));
            _operationManager.AddOperation(new Deposite(creditAccount, moneyToDeposite));
            _operationManager.AddOperation(new Deposite(depositeAccount, moneyToDeposite));
            _operationManager.ProcessOperations();
            
            Assert.True(debitAccount.Balance == moneyToDeposite);
            Assert.True(creditAccount.Balance == moneyToDeposite);
            Assert.True(depositeAccount.Balance == moneyToDeposite);
        }
        
        [Test]
        [TestCase(1000, 500)]
        public void WithdrawMoneyFromAccountAndCreditsWithdrawdedFromAccount(float moneyToDeposite, float moneyToWithDraw)
        {
            Client client = _bank.AddNewClient("First", "Client");

            DebitAccount debitAccount = _bank.OpenDebitAccount(client);
            CreditAccount creditAccount = _bank.OpenCreditAccount(client);
            DepositeAccount depositeAccount = _bank.OpenDepositeAccount(client);
            
            _operationManager.AddOperation(new Deposite(debitAccount, moneyToDeposite));
            _operationManager.AddOperation(new Deposite(creditAccount, moneyToDeposite));
            _operationManager.AddOperation(new Deposite(depositeAccount, moneyToDeposite));
            _operationManager.ProcessOperations();

            _operationManager.AddOperation(new Withdraw(debitAccount, moneyToWithDraw));
            _operationManager.AddOperation(new Withdraw(creditAccount, moneyToWithDraw));
            _operationManager.AddOperation(new Withdraw(depositeAccount, moneyToWithDraw));
            _operationManager.ProcessOperations();
            
            Assert.True(debitAccount.Balance == moneyToDeposite - moneyToWithDraw);
            Assert.True(creditAccount.Balance == moneyToDeposite - moneyToWithDraw);
            Assert.True(depositeAccount.Balance == moneyToDeposite - moneyToWithDraw);
        }

        [Test]
        [TestCase(500, 1000,200)]
        public void TransferMoneyTest(float firstBalance, float secondBalance, float moneyToTransfer)
        {
            Client client1 = _bank.AddNewClient("First", "Client");
            Client client2 = _bank.AddNewClient("Second", "Client");
            client1.AddAddress("gfgfgf"); // Для верификации
            client1.AddPassport(1623476); // Для верификации

            DebitAccount debitAccount1 = _bank.OpenDebitAccount(client1);
            DebitAccount debitAccount2 = _bank.OpenDebitAccount(client2);
            
            _operationManager.AddOperation(new Deposite(debitAccount1, firstBalance));
            _operationManager.AddOperation(new Deposite(debitAccount2, secondBalance));
            _operationManager.ProcessOperations();
            
            _operationManager.AddOperation(new Transfer(debitAccount1, debitAccount2, moneyToTransfer));
            _operationManager.ProcessOperations();
            
            Assert.True(debitAccount1.Balance == firstBalance - moneyToTransfer);
            Assert.True(debitAccount2.Balance == secondBalance + moneyToTransfer);
        }

        [Test]
        [TestCase(1000, 500, 200)]
        public void CancelTransactionTest(float firstBalance, float secondBalance, float moneyToTransfer)
        {
            Client client1 = _bank.AddNewClient("First", "Client");
            Client client2 = _bank.AddNewClient("Second", "Client");
            client1.AddAddress("gfgfgf"); // Для верификации
            client1.AddPassport(1623476); // Для верификации

            DebitAccount debitAccount1 = _bank.OpenDebitAccount(client1);
            DebitAccount debitAccount2 = _bank.OpenDebitAccount(client2);
            
            _operationManager.AddOperation(new Deposite(debitAccount1, firstBalance));
            _operationManager.AddOperation(new Deposite(debitAccount2, secondBalance));
            _operationManager.ProcessOperations();
            
            _operationManager.AddOperation(new Transfer(debitAccount1, debitAccount2, moneyToTransfer));
            _operationManager.ProcessOperations();
            
            // Проверяем что опреация была произведена
            Assert.True(debitAccount1.Balance == firstBalance - moneyToTransfer);
            Assert.True(debitAccount2.Balance == secondBalance + moneyToTransfer);
            
            _operationManager.CancelOperation(_operationManager.Transactions[_operationManager.Transactions.Count - 1].Id);
            
            // Проверяем после отвены операции вернулись ли деньги
            Assert.True(debitAccount1.Balance == firstBalance);
            Assert.True(debitAccount2.Balance == secondBalance);
        }

        [Test]
        [TestCase(1000, 50)]
        public void TimeRewindMachanismTest(float FirstSumOnBalance, int daysToRewind)
        {
            Client client = _bank.AddNewClient("First", "Client");

            DebitAccount debitAccount = _bank.OpenDebitAccount(client);
            
            _operationManager.AddOperation(new Deposite(debitAccount, FirstSumOnBalance));
            _operationManager.ProcessOperations();
            
            for (int i = 0; i < daysToRewind; i++)
            {
                debitAccount.AfterOneDay();
            }
            
            Assert.True(debitAccount.Balance == FirstSumOnBalance + (FirstSumOnBalance * _bank.BankConditions.InterestOnDebitBalance / 365) * 30); // Умножаем на 30 тк у нас выплаты раз в 30 дней происходят
        }
        
    }
}