using System.Collections.Generic;

namespace Banks
{
    public class Bank
    {
        public Bank(string name, BankConditions bankConditions)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new BankException("name is null!");
            }

            if (bankConditions == null)
            {
                throw new BankException("bankConditions is null!");
            }

            Name = name;
            BankConditions = bankConditions;
        }

        public delegate void AccountHandler(string message);
        public event AccountHandler Notify;
        public string Name { get; }
        public BankConditions BankConditions { get; }
        public List<Client> Clients { get; } = new List<Client>();

        public Client AddNewClient(string name, string surname)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new BankException("name is null!");
            }

            if (string.IsNullOrEmpty(surname))
            {
                throw new BankException("surname is null!");
            }

            Client newClient = new Client(name, surname);
            Clients.Add(newClient);
            return newClient;
        }

        public void ChangeCreditLimit(float newCreditLimit)
        {
            BankConditions.CreditLimit = newCreditLimit;
            Notify?.Invoke($"Кредитный лимит банка изменился. Теперь он сотавляет: {newCreditLimit}");
        }

        public void ChangeComission(float newComission)
        {
            BankConditions.Comission = newComission;
            Notify?.Invoke($"Комиссия за пользование кредитными средствами изменилась. Теперь она сотавляет: {newComission}");
        }

        public void ChangeUnverifiedLimit(float newUnverifiedLimit)
        {
           BankConditions.UnverifiedLimit = newUnverifiedLimit;
           Notify?.Invoke($"Сумма снятия для неверифированных пользователей изменилась. Теперь он сотавляет: {newUnverifiedLimit}");
        }

        public void ChangeBankInterest(List<Interest> newBankInterest)
        {
            if (newBankInterest == null)
            {
                throw new BankException("newBankInterest is null!");
            }

            BankConditions.BankInterest = newBankInterest;
            Notify?.Invoke($"Процент на вклады был изменен. Теперь он сотавляет: {newBankInterest}");
        }

        public void ChangeInterestOnDebitBalance(float newInterestOnDebitBalance)
        {
            BankConditions.InterestOnDebitBalance = newInterestOnDebitBalance;
            Notify?.Invoke($"Процент на остаток дебетовых счетов был изменен. Теперь он сотавляет: {newInterestOnDebitBalance}");
        }

        public void ChangeInterestOnCreditBalance(float newInterestOnCreditBalance)
        {
            BankConditions.InterestOnCreditBalance = newInterestOnCreditBalance;
            Notify?.Invoke($"Процент на остаток кредитных счетов был изменен. Теперь он сотавляет: {newInterestOnCreditBalance}");
        }

        public void ChangeTimeToWithdrawOnDepositeAccount(int newTimeToWithdrawOnDepositeAccount)
        {
            BankConditions.TimeToWithdrawOnDepositeAccount = newTimeToWithdrawOnDepositeAccount;
            Notify?.Invoke($"Ограничение времени на вывод было изменено. Теперь оно сотавляет: {newTimeToWithdrawOnDepositeAccount}");
        }

        public DebitAccount OpenDebitAccount(Client client)
        {
            if (client == null)
            {
                throw new BankException("client is null!");
            }

            DebitAccount debitAccount = new DebitAccount(BankConditions, client.IsVerify);
            client.Accounts.Add(debitAccount);
            return debitAccount;
        }

        public DepositeAccount OpenDepositeAccount(Client client)
        {
            if (client == null)
            {
                throw new BankException("client is null!");
            }

            DepositeAccount depositeAccount =
                new DepositeAccount(BankConditions, client.IsVerify);
            client.Accounts.Add(depositeAccount);
            return depositeAccount;
        }

        public CreditAccount OpenCreditAccount(Client client)
        {
            if (client == null)
            {
                throw new BankException("client is null!");
            }

            CreditAccount creditAccount = new CreditAccount(BankConditions, client.IsVerify);
            client.Accounts.Add(creditAccount);
            return creditAccount;
        }

        public void VerifyClient(Client client, string address, int passportId)
        {
            if (client == null)
            {
                throw new BankException("client is null!");
            }

            if (string.IsNullOrEmpty(address))
            {
                throw new BankException("name is null!");
            }

            client.AddAddress(address);
            client.AddPassport(passportId);
            foreach (var account in client.Accounts)
            {
                account.IsVerify = true;
            }
        }

        public void SubcribeClientOnNotify(Client client)
        {
            if (client == null)
            {
                throw new BankException("client is null!");
            }

            Notify += client.AddMessage;
        }

        public void UnSubscribe(Client client)
        {
            if (client == null)
            {
                throw new BankException("client is null!");
            }

            Notify -= client.AddMessage;
        }
    }
}