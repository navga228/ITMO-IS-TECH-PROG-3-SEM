using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;

namespace Banks
{
    internal static class Program
    {
        private static CentralBank _centralBank = new CentralBank("ЦБРФ");
        private static OperationManager _operationManager = new OperationManager();
        private static List<Interest> interests = new List<Interest>() { new Interest(0, 50000, 1.00f), new Interest(50000, float.MaxValue, 2.00f) };
        private static BankConditions bankConditions = new BankConditions(interests, 100000, 50000, 500, 1.00f, 1.00f, 0);
        private static Bank _bank = _centralBank.AddNewBank("Сбер", bankConditions);
        private static Client _client;
        private static Client _clientForTransferTest = new Client("Тестовый", "Тест");

        private static IAccount _debitAccountForTranferTest = _bank.OpenDebitAccount(_clientForTransferTest);
        private static int ParceToInt(string str)
        {
            if (int.TryParse(str, out var number))
            {
                return number;
            }

            Console.WriteLine("Вы ввели некоректное число");
            return -1;
        }

        private static float ParceToFloat(string str)
        {
            if (float.TryParse(str, out var number))
            {
                return number;
            }

            Console.WriteLine("Вы ввели некоректное число");
            return -1;
        }

        private static void Main()
        {
            Console.WriteLine("Здравствуйте, вам нужно создать аккаунт");
            Console.WriteLine("Введите имя");
            var name = Console.ReadLine();
            Console.WriteLine("Введите фамилию");
            var surname = Console.ReadLine();
            _client = new Client(name, surname);
            _bank.OpenDebitAccount(_client);
            _bank.OpenDepositeAccount(_client);
            _bank.OpenCreditAccount(_client);
            bool mainMenu = true;
            bool accountVerify = false;
            while (mainMenu)
            {
                Console.WriteLine("-----------------------------------ГЛАВНОЕ МЕНЮ-----------------------------------");
                Console.WriteLine("Выберите дейсвие");
                if (!accountVerify) Console.WriteLine("Если вы хотите верифицировать аккаунт, нажмите 1");
                Console.WriteLine("Чтобы перейти в дебетовый счет, нажмите 2");
                Console.WriteLine("Чтобы перейти в депозитный счет, нажмите 3");
                Console.WriteLine("Чтобы перейти в кредитный счет, нажмите 4");
                Console.WriteLine("Чтобы подписаться на уведомления банка, нажмите 5");
                Console.WriteLine("Чтобы отписаться от уведомлений банка, нажмите 6");
                Console.WriteLine("Чтобы получить информацию о всех счетах, нажмите 7");
                Console.WriteLine("Чтобы закрыть приложение, нажмите 8");
                Console.WriteLine("-----------------------------------------------------------------------------------");
                switch (ParceToInt(Console.ReadLine()))
                {
                    case 1:
                        Console.WriteLine("Введите контактный адрес");
                        string address = Console.ReadLine();
                        Console.WriteLine("Введите паспортные данные");
                        _bank.VerifyClient(_client, address, ParceToInt(Console.ReadLine()));
                        accountVerify = true;
                        Console.WriteLine("Ваш аккаунт успешно верифицирован!");
                        break;
                    case 2:
                        bool debitMenu = true;
                        while (debitMenu)
                        {
                            Console.WriteLine("-----------------------------------МЕНЮ ДЕБЕТОВОГО СЧЕТА-----------------------------------");
                            Console.WriteLine("Выберите действие");
                            Console.WriteLine("Чтобы первести деньги другому человеку, нажмите 1");
                            Console.WriteLine("Чтобы снять деньги со счета, нажмите 2");
                            Console.WriteLine("Чтобы внести деньги, нажмите 3");
                            Console.WriteLine("Чтобы просмотреть баланс счета, нажмите 4");
                            Console.WriteLine("Чтобы выйти в главное меню, нажмите 5");
                            Console.WriteLine("-------------------------------------------------------------------------------------------");
                            IAccount account = _client.Accounts.Where(acc => acc.Name == "DebitCard").FirstOrDefault(); // ищем дебетовый акк клиента
                            switch (ParceToInt(Console.ReadLine()))
                            {
                                case 1:
                                    Console.WriteLine("Введите сумму, которую хотите перевести");
                                    float sum1 = ParceToFloat(Console.ReadLine());
                                    _operationManager.AddOperation(new Transfer(account, _debitAccountForTranferTest, sum1));
                                    _operationManager.ProcessOperations();
                                    Console.WriteLine($"{sum1} переведено успешно!");
                                    break;
                                case 2:
                                    Console.WriteLine("Введите сумму, которую хотите снять");
                                    float sum2 = ParceToFloat(Console.ReadLine());
                                    _operationManager.AddOperation(new Withdraw(account, sum2));
                                    _operationManager.ProcessOperations();
                                    Console.WriteLine($"{sum2} успешно снято!");
                                    break;
                                case 3:
                                    Console.WriteLine("Введите сумму, которую хотите внести");
                                    float sum3 = ParceToFloat(Console.ReadLine());
                                    _operationManager.AddOperation(new Deposite(account, sum3));
                                    _operationManager.ProcessOperations();
                                    Console.WriteLine($"{sum3} успешно внесены!");
                                    break;
                                case 4:
                                    Console.WriteLine($"Ваш баланс дебетового счета {account.Balance}");
                                    break;
                                case 5:
                                    debitMenu = false;
                                    break;
                            }
                        }

                        break;
                    case 3:
                        bool depositeMenu = true;
                        while (depositeMenu)
                        {
                            Console.WriteLine("-----------------------------------МЕНЮ ДЕПОЗИТНОГО СЧЕТА-----------------------------------");
                            Console.WriteLine("Выберите действие");
                            Console.WriteLine("Чтобы первести деньги другому человеку, нажмите 1");
                            Console.WriteLine("Чтобы снять деньги со счета, нажмите 2");
                            Console.WriteLine("Чтобы внести деньги, нажмите 3");
                            Console.WriteLine("Чтобы просмотреть баланс счета, нажмите 4");
                            Console.WriteLine("Чтобы выйти в главное меню, нажмите 5");
                            Console.WriteLine("--------------------------------------------------------------------------------------------");
                            IAccount account = _client.Accounts.Where(acc => acc.Name == "DepositeCard").FirstOrDefault(); // ищем депозитный акк клиента
                            switch (ParceToInt(Console.ReadLine()))
                            {
                                case 1:
                                    Console.WriteLine("Введите сумму, которую хотите перевести");
                                    float sum1 = ParceToFloat(Console.ReadLine());
                                    _operationManager.AddOperation(new Transfer(account, _debitAccountForTranferTest, sum1));
                                    _operationManager.ProcessOperations();
                                    Console.WriteLine($"{sum1} переведено успешно!");
                                    break;
                                case 2:
                                    Console.WriteLine("Введите сумму, которую хотите снять");
                                    float sum2 = ParceToFloat(Console.ReadLine());
                                    _operationManager.AddOperation(new Withdraw(account, sum2));
                                    _operationManager.ProcessOperations();
                                    Console.WriteLine($"{sum2} успешно снято!");
                                    break;
                                case 3:
                                    Console.WriteLine("Введите сумму, которую хотите внести");
                                    float sum3 = ParceToFloat(Console.ReadLine());
                                    _operationManager.AddOperation(new Deposite(account, sum3));
                                    _operationManager.ProcessOperations();
                                    Console.WriteLine($"{sum3} успешно внесены!");
                                    break;
                                case 4:
                                    Console.WriteLine($"Ваш баланс депозитного счета {account.Balance}");
                                    break;
                                case 5:
                                    depositeMenu = false;
                                    break;
                            }
                        }

                        break;
                    case 4:
                        bool creditMenu = true;
                        while (creditMenu)
                        {
                            Console.WriteLine("-----------------------------------МЕНЮ КРЕДИТНОГО СЧЕТА-----------------------------------");
                            Console.WriteLine("Выберите действие");
                            Console.WriteLine("Чтобы первести деньги другому человеку, нажмите 1");
                            Console.WriteLine("Чтобы снять деньги со счета, нажмите 2");
                            Console.WriteLine("Чтобы внести деньги, нажмите 3");
                            Console.WriteLine("Чтобы просмотреть баланс счета, нажмите 4");
                            Console.WriteLine("Чтобы выйти в главное меню, нажмите 5");
                            Console.WriteLine("--------------------------------------------------------------------------------------------");
                            IAccount account = _client.Accounts.Where(acc => acc.Name == "CreditCard").FirstOrDefault(); // ищем кредитный акк клиента
                            switch (ParceToInt(Console.ReadLine()))
                            {
                                case 1:
                                    Console.WriteLine("Введите сумму, которую хотите перевести");
                                    float sum1 = ParceToFloat(Console.ReadLine());
                                    _operationManager.AddOperation(new Transfer(account, _debitAccountForTranferTest, sum1));
                                    _operationManager.ProcessOperations();
                                    Console.WriteLine($"{sum1} перведено успешно!");
                                    break;
                                case 2:
                                    Console.WriteLine("Введите сумму, которую хотите снять");
                                    float sum2 = ParceToFloat(Console.ReadLine());
                                    _operationManager.AddOperation(new Withdraw(account, sum2));
                                    _operationManager.ProcessOperations();
                                    Console.WriteLine($"{sum2} успешно снято!");
                                    break;
                                case 3:
                                    Console.WriteLine("Введите сумму, которую хотите внести");
                                    float sum3 = ParceToFloat(Console.ReadLine());
                                    _operationManager.AddOperation(new Deposite(account, sum3));
                                    _operationManager.ProcessOperations();
                                    Console.WriteLine($"Счет был пополнен на {sum3}");
                                    break;
                                case 4:
                                    Console.WriteLine($"Ваш баланс депозитного счета {account.Balance}");
                                    break;
                                case 5:
                                    creditMenu = false;
                                    break;
                            }
                        }

                        break;
                    case 5:
                        _bank.SubcribeClientOnNotify(_client);
                        Console.WriteLine("Вы успешно подписались на уведомлений!");
                        break;
                    case 6:
                        _bank.UnSubscribe(_client);
                        Console.WriteLine("Вы успешно отписались от уведомлений!");
                        break;
                    case 7:
                        Console.WriteLine();
                        foreach (var account in _client.Accounts)
                        {
                            Console.WriteLine($"{account.Name}");
                            Console.WriteLine($"Balance {account.Balance}");
                            Console.WriteLine($"Card number {account.CardNumber}");
                            Console.WriteLine(" ");
                        }

                        break;
                    case 8:
                        mainMenu = false;
                        Console.WriteLine("До скорых встреч!");
                        break;
                }
            }
        }
    }
}
