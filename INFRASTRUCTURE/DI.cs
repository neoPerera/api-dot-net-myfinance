using APPLICATION.Interfaces;
using APPLICATION.Services;
using CORE.Interfaces;
using INFRASTRUCTURE.Persistence;
using INFRASTRUCTURE.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace INFRASTRUCTURE
{
    public static class DI
    {
        // Register Application Services
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ILogin, LoginService>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            services.AddScoped<IFormService, FormService>();
            services.AddScoped<IFormRepository, FormRepository>();

            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();

            services.AddScoped<IIncomeService, IncomeService>();
            services.AddScoped<IIncomeRepository, IncomeRepository>();

            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            return services;
        }

        // Register Infrastructure Services (Database & Repositories)
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("Database connection string is missing from configuration.");
            }

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));
            return services;
        }
    }
}
