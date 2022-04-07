using System.Collections.Generic;

namespace Banks
{
    public class BankConditions
    { // Done
        public BankConditions(List<Interest> bankInterest, float unverifiedLimit, float creditLimit, float comission, float interestOnDebitBalance, float interestOnCreditBalance, int timeToWithdrawOnDepositeAccount)
        {
            if (bankInterest == null)
            {
                throw new BankException("bankInterest is null");
            }

            BankInterest = bankInterest;
            UnverifiedLimit = unverifiedLimit;
            CreditLimit = creditLimit;
            Comission = comission;
            InterestOnDebitBalance = interestOnDebitBalance;
            InterestOnCreditBalance = interestOnCreditBalance;
            TimeToWithdrawOnDepositeAccount = timeToWithdrawOnDepositeAccount;
        }

        public List<Interest> BankInterest { get; set; } // Банковский процент на вклады
        public float UnverifiedLimit { get; set; } // Лемит на снятие для неверифицированых пользователей
        public float CreditLimit { get; set; } // Кредитный лимит
        public float Comission { get; set; } // Комиссия за пользование кредитными деньгами
        public float InterestOnDebitBalance { get; set; } // Процент на остаток дебетового счета
        public float InterestOnCreditBalance { get; set; } // Процент на остаток кредитного счета
        public int TimeToWithdrawOnDepositeAccount { get; set; } // Время после которого можно выводить деньги с депозитного аккаунта
    }
}