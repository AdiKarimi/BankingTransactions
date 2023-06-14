using Banking.Framework.Types;

namespace Banking.Framework.Domain
{
    public class AccountTransaction
    {
        public int AccountNumber { get; set; }
        public TransactionType TransactionType { get; set; }
        public Money Amount { get; set; }
    }
}
