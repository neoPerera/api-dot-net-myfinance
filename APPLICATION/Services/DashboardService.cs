using APPLICATION.DTOs;
using APPLICATION.Interfaces;
using CORE.Entities;
using CORE.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace APPLICATION.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardService(IHttpContextAccessor httpContextAccessor,IDashboardRepository dashboardRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _dashboardRepository = dashboardRepository;
        }
        public async Task<CommonResponse> GetDashboardAccountBalancesAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User.Identity;
            var dashboardChart4 = await _dashboardRepository.GetDashboardChart4DataAsync(user?.Name ?? "ERROR");

            // Calculate account balances
            var accountBalances = dashboardChart4
                .GroupBy(r => r.Account)
                .Select(g => new DashboardChart4AccountBalance
                {
                    Account_name = g.Key,
                    Account_balance = g.Sum(x => x.IntAmount ?? 0)
                }).ToList();

            return new ResponseService<CommonResponse>(accountBalances).Response;

        }
        public async Task<CommonResponse> GetDashboardAccountIncomesAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User.Identity;
            var dashboardIncomes = await _dashboardRepository.GetDashboardIncomeAsync(user?.Name ?? "ERROR");
            return new ResponseService<CommonResponse>(dashboardIncomes).Response;
        }

        public async Task<CommonResponse> GetDashboardAccountExpensesAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User.Identity;
            var dashboardExpenses = await _dashboardRepository.GetDashboardExpensesAsync(user?.Name ?? "ERROR");
            return new ResponseService<CommonResponse>(dashboardExpenses).Response;
        }

        public async Task<CommonResponse> GetDashboardTransactionsAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User.Identity;
            var dashboardTransactions = await _dashboardRepository.GetDashboardChart4DataAsync(user?.Name ?? "ERROR");
            return new ResponseService<CommonResponse>(dashboardTransactions).Response;
        }
    }
}
