using Banking.Transactions.Models;

namespace Banking.Transactions.Services
{
    public interface IIdentityService
    {
        IdentityModel GetIdentity();
    }
}
