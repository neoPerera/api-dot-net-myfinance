﻿using APPLICATION.Interfaces;
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
            services.AddScoped<ICommonRepository, CommonRepository>();

            services.AddScoped<ILogin, LoginService>();
            
            services.AddScoped<IFormService, FormService>();

            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();

            services.AddScoped<IIncomeService, IncomeService>();

            services.AddScoped<IExpenseService, ExpenseService>();

            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<ITransactionService, TransactionService>();

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
