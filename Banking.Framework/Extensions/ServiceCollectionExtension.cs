using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Banking.Framework.Data;
using Banking.Framework.Data.Interface;
using Banking.Framework.Data.Repositories;
using Banking.Framework.Services;
using Banking.Framework.Services.Interface;
using Banking.Framework.Mappers;
using AutoMapper;

namespace Banking.Framework.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddTransactionFramework(this IServiceCollection services, IConfiguration configuration)
        {
            // Service
            services.AddScoped<ITransactionService, TransactionService>();

            // Repository
            services.AddScoped<IAccountSummaryRepository, AccountSummaryRepository>();
            services.AddScoped<IAccountTransactionRepository, AccountTransactionRepository>();

            // Mappers
            services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));

            // Connection String
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection")));

            return services;
        }
    }
}
