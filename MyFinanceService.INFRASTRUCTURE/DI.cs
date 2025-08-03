using MyFinanceService.APPLICATION.Interfaces;
using MyFinanceService.APPLICATION.Services;
using MyFinanceService.INFRASTRUCTURE.Repositories;
using MyFinanceService.INFRASTRUCTURE.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using MyFinanceService.CORE.Interfaces;

namespace MyFinanceService.INFRASTRUCTURE
{
    public static class DI
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            
            // Business Logic Services
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IIncomeService, IncomeService>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IDashboardService, DashboardService>();
            
            // Common Services
            services.AddScoped<ICommonRepository, CommonRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            
            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("Database connection string is missing from configuration.");
            }

            services.AddDbContext<MyFinanceDbContext>(options =>
                options.UseNpgsql(connectionString));
            
            return services;
        }
    }
}
