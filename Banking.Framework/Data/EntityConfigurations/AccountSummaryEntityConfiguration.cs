using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Banking.Framework.Data.Entities;

namespace Banking.Framework.Data.EntityConfigurations
{
    public static class AccountSummaryEntityConfiguration
    {
        public static void Configure(EntityTypeBuilder<AccountSummaryEntity> entityBuilder)
        {
            entityBuilder.HasKey(t => t.AccountNumber);
            entityBuilder.Property(t => t.Balance).IsConcurrencyToken().IsRequired();
            entityBuilder.Property(t => t.Currency).IsRequired(); 
        }
    }
}
