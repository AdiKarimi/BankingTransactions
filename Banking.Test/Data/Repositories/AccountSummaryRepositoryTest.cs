using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Banking.Framework.Data;
using Banking.Framework.Data.Entities;
using Banking.Framework.Data.Repositories;
using Banking.Framework.Mappers;
using Xunit;

namespace Banking.Test.Data.Repositories
{
    public class AccountSummaryRepositoryTest
    {
        protected AccountSummaryRepository AccountSummaryRepositoryUnderTest { get; set; }
        protected ApplicationDbContext DbContextInMemory { get; }
        protected MapperConfiguration MappingConfig { get; }
        protected IMapper Mapper { get; }

        public AccountSummaryRepositoryTest()
        {
            DbContextInMemory = GetInMemoryDbContext();
            MappingConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new MappingProfile()); });
            Mapper = MappingConfig.CreateMapper();
            AccountSummaryRepositoryUnderTest = new AccountSummaryRepository(DbContextInMemory);
        }

        public class Read : AccountSummaryRepositoryTest
        {
            [Fact]
            public void Should_return_accountsummary_when_accountnumber_exist()
            {
                // Arrange
                int existingAccountNumber = 2398;

                // Act
                var result = AccountSummaryRepositoryUnderTest.Read(existingAccountNumber).Result;

                // Assert
                Assert.Equal(AccountSummaryDataEntity.AccountNumber, result.AccountNumber);
                Assert.Equal(AccountSummaryDataEntity.Balance, result.Balance);
                Assert.Equal(AccountSummaryDataEntity.Currency, result.Currency);
            }

            [Fact]
            public void Should_return_null_when_accountnumber_doesnotexist()
            {
                // Arrange
                int invalidAccountNumber = 23981;
                AccountSummaryEntity expectedResult = null;

                // Act
                var actualResult = AccountSummaryRepositoryUnderTest.Read(invalidAccountNumber).Result;

                // Assert
                Assert.Equal(expectedResult, actualResult);
            }
        }

        private static ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                      .UseInMemoryDatabase("bankingtransactionsdb")
                      .Options;
            var context = new ApplicationDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var accountSummaryDataEntity = AccountSummaryDataEntity;
            context.Add(accountSummaryDataEntity);
            context.SaveChanges();

            return context;
        }

        protected static AccountSummaryEntity AccountSummaryDataEntity => new AccountSummaryEntity()
        {
            AccountNumber = 2398,
            Balance = 10000,
            Currency = "INR"
        };
    }    
}
