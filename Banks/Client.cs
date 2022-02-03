using System.Collections.Generic;

namespace Banks
{
    public class Client
    {
        private string _name;
        private string _surname;
        private string _address;
        private int _passportId;
        private Bank _bank;
        private List<string> _messages = new List<string>();

        public Client(string name, string surname, Bank bank)
        {
            _name = name;
            _surname = surname;
            _bank = bank;
        }

        public List<IAccount> Accounts { get; } = new List<IAccount>();
        public Bank Bank
        {
            get => _bank;
        }

        public bool IsVerify => !string.IsNullOrEmpty(_name) && !string.IsNullOrEmpty(_surname) && !string.IsNullOrEmpty(_address) && !(_passportId == default(int)) && !(_bank is null);

        private string Passport { get; set; }
        public void AddAddress(string address)
        {
            _address = address;
        }

        public void AddPassport(int passportId)
        {
            _passportId = passportId;
        }

        public void OpenDebitAccount()
        {
            _bank.OpenDebitAccount(this);
        }

        public void OpenDepositeAccount()
        {
            _bank.OpenDepositeAccount(this);
        }

        public void OpenCreditAccount()
        {
            _bank.OpenCreditAccount(this);
        }

        public void AddMessage(string message)
        {
            _messages.Add(message);
        }
    }
}