using Banking.Transactions.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Banking.Framework.Extensions;
using Banking.Transactions.Services;

namespace Banking.Transactions
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransactionFramework(Configuration);
            services.AddScoped<ExceptionHandlerMiddleware>();
            services.AddScoped<IIdentityService, IdentityService>();
            //services.AddApplicationInsightsTelemetry(Configuration);
            //AutoMapper.Mapper.Initialize(config =>
            //{
            //    config.CreateMap<TransactionModel, AccountTransaction>().AfterMap<SetIdentityAction>()
            //     .ForAllMembers(opts => opts.Ignore());

            //    config.CreateMap<TransactionResult, TransactionResultModel>()
            //    .ForMember(dest => dest.Balance, opt => opt.MapFrom(o => o.Balance.Amount.ToString("N")))
            //    .ForMember(dest => dest.Currency, opt => opt.MapFrom(o => o.Balance.Currency.ToString()));

            //});

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "The Banking Transactions", Version = "v1" });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory log)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandlerMiddleware();
            log.AddApplicationInsights(app.ApplicationServices, LogLevel.Information);
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "The Banking Transactions v1");
            });
            app.UseMvc();
        }
    }
}
