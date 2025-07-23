using APPLICATION.DTOs;

namespace APPLICATION.Interfaces
{
    public interface IDashboardService
    {
        Task<CommonResponse> GetDashboardAccountBalancesAsync();
        Task<CommonResponse> GetDashboardAccountIncomesAsync();
        Task<CommonResponse> GetDashboardAccountExpensesAsync();
        Task<CommonResponse> GetDashboardTransactionsAsync();
    }
}
