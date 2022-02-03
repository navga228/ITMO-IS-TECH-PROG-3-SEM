using System.Collections.Generic;

namespace Banks
{
    public class Bank
    {
        public Bank(string name, List<Interest> bankInterest, float unverifiedLimit, float creditLimit, float comission, float interestOnDebitBalance, float interestOnCreditBalance, int timeToWithdrawOnDepositeAccount)
        {
            Name = name;
            BankInterest = bankInterest;
            UnverifiedLimit = unverifiedLimit;
            CreditLimit = creditLimit;
            Comission = comission;
            InterestOnDebitBalance = interestOnDebitBalance;
            InterestOnCreditBalance = interestOnCreditBalance;
            TimeToWithdrawOnDepositeAccount = timeToWithdrawOnDepositeAccount;
        }

        public delegate void AccountHandler(string message);
        public event AccountHandler Notify;
        public string Name { get; }
        public List<Client> Clients { get; } = new List<Client>();
        public float UnverifiedLimit { get; set; } // Лемит на снятие для неверифицированых пользователей
        public List<Interest> BankInterest { get; set; } // Банковский процент на вклады
        public float CreditLimit { get; set; } // Кредитный лимит
        public float Comission { get; set; } // Комиссия за пользование кредитными деньгами
        public float InterestOnDebitBalance { get; set; } // Процент на остаток дебетового счета
        public float InterestOnCreditBalance { get; set; } // Процент на остаток кредитного счета
        public int TimeToWithdrawOnDepositeAccount { get; set; } // Время после которого можно выводить деньги с депозитного аккаунта

        public Client AddNewClient(string name, string surname)
        {
            Client newClient = new Client(name, surname, this);
            Clients.Add(newClient);
            return newClient;
        }

        public void ChangeCreditLimit(float newCreditLimit)
        {
            CreditLimit = newCreditLimit;
            Notify?.Invoke($"Кредитный лимит банка изменился. Теперь он сотавляет: {newCreditLimit}");
        }

        public void ChangeComission(float newComission)
        {
            Comission = newComission;
            Notify?.Invoke($"Комиссия за пользование кредитными средствами изменилась. Теперь она сотавляет: {newComission}");
        }

        public void ChangeUnverifiedLimit(float newUnverifiedLimit)
        {
            UnverifiedLimit = newUnverifiedLimit;
            Notify?.Invoke($"Сумма снятия для неверифированных пользователей изменилась. Теперь он сотавляет: {newUnverifiedLimit}");
        }

        public void ChangeBankInterest(List<Interest> newBankInterest)
        {
            BankInterest = newBankInterest;
            Notify?.Invoke($"Процент на вклады был изменен. Теперь он сотавляет: {newBankInterest}");
        }

        public void ChangeInterestOnDebitBalance(float newInterestOnDebitBalance)
        {
            InterestOnDebitBalance = newInterestOnDebitBalance;
            Notify?.Invoke($"Процент на остаток дебетовых счетов был изменен. Теперь он сотавляет: {InterestOnDebitBalance}");
        }

        public void ChangeInterestOnCreditBalance(float newInterestOnCreditBalance)
        {
            InterestOnCreditBalance = newInterestOnCreditBalance;
            Notify?.Invoke($"Процент на остаток кредитных счетов был изменен. Теперь он сотавляет: {InterestOnCreditBalance}");
        }

        public void ChangeTimeToWithdrawOnDepositeAccount(int newTimeToWithdrawOnDepositeAccount)
        {
            TimeToWithdrawOnDepositeAccount = newTimeToWithdrawOnDepositeAccount;
            Notify?.Invoke($"Ограничение времени на вывод было изменено. Теперь оно сотавляет: {TimeToWithdrawOnDepositeAccount}");
        }

        public DebitAccount OpenDebitAccount(Client client)
        {
            DebitAccount debitAccount = new DebitAccount(client, InterestOnDebitBalance);
            client.Accounts.Add(debitAccount);
            return debitAccount;
        }

        public DepositeAccount OpenDepositeAccount(Client client)
        {
            DepositeAccount depositeAccount =
                new DepositeAccount(client, BankInterest, TimeToWithdrawOnDepositeAccount);
            client.Accounts.Add(depositeAccount);
            return depositeAccount;
        }

        public CreditAccount OpenCreditAccount(Client client)
        {
            CreditAccount creditAccount = new CreditAccount(client, CreditLimit, Comission, InterestOnCreditBalance);
            client.Accounts.Add(creditAccount);
            return creditAccount;
        }

        public void SubcribeClientOnNotify(Client client)
        {
            Notify += client.AddMessage;
        }

        public void UnSubscribe(Client client)
        {
            Notify -= client.AddMessage;
        }
    }
}