using System.Threading.Tasks;
using Banking.Framework.Data.Entities;

namespace Banking.Framework.Data.Interface
{
    public interface IAccountTransactionRepository
    {
        Task Create(AccountTransactionEntity accountTransactionEntity, AccountSummaryEntity accountSummaryEntity);
    }
}
