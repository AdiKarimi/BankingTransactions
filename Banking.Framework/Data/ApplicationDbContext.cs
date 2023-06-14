using Microsoft.EntityFrameworkCore;
using Banking.Framework.Data.Entities;
using Banking.Framework.Data.EntityConfigurations;

namespace Banking.Framework.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AccountSummaryEntityConfiguration
                .Configure(modelBuilder.Entity<AccountSummaryEntity>());

            AccountTransactionEntityConfiguration
                .Configure(modelBuilder.Entity<AccountTransactionEntity>());

            base.OnModelCreating(modelBuilder);
        }
    }
}
