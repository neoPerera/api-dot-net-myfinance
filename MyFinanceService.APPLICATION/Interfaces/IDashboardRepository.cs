using MyFinanceService.APPLICATION.DTOs;

namespace MyFinanceService.APPLICATION.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<DashboardTypeValue>> GetDashboardChart1DataAsync(string userId);
        Task<List<DashboardChart2>> GetDashboardChart2DataAsync(string userId);
        Task<List<DashboardTypeValue>> GetDashboardChart3DataAsync(string userId);
        Task<List<DashboardChart4>> GetDashboardChart4DataAsync(string userId);
        Task<List<DashboardTypeValue>> GetDashboardExpensesAsync(string userId);

        Task<List<DashboardTypeValue>> GetDashboardIncomeAsync(string userId);

    }
}
