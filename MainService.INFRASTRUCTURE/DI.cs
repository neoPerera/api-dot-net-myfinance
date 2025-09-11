
using MainService.APPLICATION.Interfaces;
using MainService.APPLICATION.Services;
using MainService.CORE.Interfaces;
using MainService.INFRASTRUCTURE.Persistence;
using MainService.INFRASTRUCTURE.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MainService.INFRASTRUCTURE
{
    public static class DI
    {
        // Register Application Services
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICommonRepository, CommonRepository>();

            services.AddScoped<ILogin, LoginService>();
            
            services.AddScoped<IFormService, FormService>();

            services.AddScoped<IActivityLogService, ActivityLogService>();
            services.AddScoped<IKafkaProducerService, KafkaProducerService>();

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
