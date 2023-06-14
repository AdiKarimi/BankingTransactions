using System.Threading.Tasks;
using Banking.Framework.Data.Entities;

namespace Banking.Framework.Data.Interface
{
    public interface IAccountSummaryRepository
    {
        Task<AccountSummaryEntity> Read(int accountNumber);
    }
}
