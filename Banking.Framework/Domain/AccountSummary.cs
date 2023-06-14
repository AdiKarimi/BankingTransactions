using Banking.Framework.Types;

namespace Banking.Framework.Domain
{
    public class AccountSummary
    {
        public int AccountNumber { get; set; }
        public Money Balance { get; set; }
    }
}
