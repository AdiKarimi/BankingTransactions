namespace Banking.Client.Models
{
    public class TransactionInput
    {
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
    }
}
