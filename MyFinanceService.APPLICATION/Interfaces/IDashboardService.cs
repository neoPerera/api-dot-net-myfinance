using MyFinanceService.APPLICATION.DTOs;

namespace MyFinanceService.APPLICATION.Interfaces
{
    public interface IDashboardService
    {
        Task<CommonResponse> GetDashboardAccountBalancesAsync();
        Task<CommonResponse> GetDashboardAccountIncomesAsync();
        Task<CommonResponse> GetDashboardAccountExpensesAsync();
        Task<CommonResponse> GetDashboardTransactionsAsync();
    }
}
