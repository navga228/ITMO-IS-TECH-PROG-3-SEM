using System.Collections.Generic;

namespace Banks
{
    public class Client
    {
        private string _name;
        private string _surname;
        private string _address;
        private int _passportId;
        private List<string> _messages = new List<string>();

        public Client(string name, string surname)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new BankException("name null or empty!");
            }

            if (string.IsNullOrEmpty(surname))
            {
                throw new BankException("surname null or empty!");
            }

            _name = name;
            _surname = surname;
        }

        public List<IAccount> Accounts { get; } = new List<IAccount>();

        public bool IsVerify => !string.IsNullOrEmpty(_name) && !string.IsNullOrEmpty(_surname) && !string.IsNullOrEmpty(_address) && !(_passportId == default(int));

        private string Passport { get; set; }
        public void AddAddress(string address)
        {
            _address = address;
        }

        public void AddPassport(int passportId)
        {
            _passportId = passportId;
        }

        public void AddMessage(string message)
        {
            _messages.Add(message);
        }
    }
}