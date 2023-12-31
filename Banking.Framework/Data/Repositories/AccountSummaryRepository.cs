﻿using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Banking.Framework.Data.Entities;
using Banking.Framework.Data.Interface;

namespace Banking.Framework.Data.Repositories
{
    public class AccountSummaryRepository : IAccountSummaryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<AccountSummaryEntity> _accountSummaryEntity;

        public AccountSummaryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _accountSummaryEntity = _dbContext.Set<AccountSummaryEntity>();
        }

        public async Task<AccountSummaryEntity> Read(int accountNumber)
        {
            return await _accountSummaryEntity.AsNoTracking()
                .FirstOrDefaultAsync(e => e.AccountNumber == accountNumber);
        }
    }
}
