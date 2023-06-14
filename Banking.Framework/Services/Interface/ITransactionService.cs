using System.Threading.Tasks;
using Banking.Framework.Domain;

namespace Banking.Framework.Services.Interface
{
    public interface ITransactionService
    {
        Task<TransactionResult> Balance(int accountNumber);
        Task<TransactionResult> Deposit(AccountTransaction accountTransaction);
        Task<TransactionResult> Withdraw(AccountTransaction accountTransaction);
    }
}
