using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Banking.Framework.Data.Entities;
using Banking.Framework.Data.Interface;
using Banking.Framework.Exceptions;
using Banking.Framework.Types;

namespace Banking.Framework.Data.Repositories
{
    public class AccountTransactionRepository : IAccountTransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<AccountSummaryEntity> _accountSummaryEntity;
        private readonly DbSet<AccountTransactionEntity> _accountTransactionEntity;        

        public AccountTransactionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _accountSummaryEntity = _dbContext.Set<AccountSummaryEntity>();
            _accountTransactionEntity = _dbContext.Set<AccountTransactionEntity>();
        }

        public async Task Create(AccountTransactionEntity accountTransactionEntity, AccountSummaryEntity accountSummaryEntity)
        {
            _accountTransactionEntity.Add(accountTransactionEntity);
            _accountSummaryEntity.Update(accountSummaryEntity);

            bool isSaved = false;

            while (!isSaved)
            {
                try
                {
                    await _dbContext.SaveChangesAsync();
                    isSaved = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is AccountSummaryEntity)
                        {
                            var databaseValues = entry.GetDatabaseValues();

                            if (databaseValues != null)
                            {
                                entry.OriginalValues.SetValues(databaseValues);
                                CalculateNewBalance();

                                void CalculateNewBalance()
                                {
                                    var balance = (decimal)entry.OriginalValues["Balance"];
                                    var amount = accountTransactionEntity.Amount;

                                    if (accountTransactionEntity.TransactionType == TransactionType.Deposit.ToString())
                                    {
                                        accountSummaryEntity.Balance =
                                        balance += amount;
                                    }
                                    else if (accountTransactionEntity.TransactionType == TransactionType.Withdrawal.ToString())
                                    {
                                        if(amount > balance)
                                            throw new InsufficientBalanceException();

                                        accountSummaryEntity.Balance =
                                        balance -= amount;
                                    }
                                }
                            }
                            else
                            {
                                throw new NotSupportedException();
                            }
                        }
                    }
                }
            }            
        }
    }
}
