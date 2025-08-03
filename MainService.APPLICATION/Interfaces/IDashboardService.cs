using MainService.APPLICATION.DTOs;

namespace MainService.APPLICATION.Interfaces
{
    public interface IDashboardService
    {
        Task<CommonResponse> GetDashboardAccountBalancesAsync();
        Task<CommonResponse> GetDashboardAccountIncomesAsync();
        Task<CommonResponse> GetDashboardAccountExpensesAsync();
        Task<CommonResponse> GetDashboardTransactionsAsync();
    }
}
