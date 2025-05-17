using APPLICATION.DTOs;
using APPLICATION.Interfaces;
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

        public async Task<DashboardResponse> GetDashboardDataAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User.Identity;
            // Fetch the data from the repository (returning the list of entities)
            var dashboardChart1 = await _dashboardRepository.GetDashboardChart1DataAsync(user?.Name ?? "ERROR");
            var dashboardChart2 = await _dashboardRepository.GetDashboardChart2DataAsync(user?.Name ?? "ERROR");
            var dashboardChart3 = await _dashboardRepository.GetDashboardChart3DataAsync(user?.Name ?? "ERROR");
            var dashboardChart4 = await _dashboardRepository.GetDashboardChart4DataAsync(user?.Name ?? "ERROR");
            var dashboardExpenses = await _dashboardRepository.GetDashboardExpensesAsync(user?.Name ?? "ERROR");
            var dashboardIncomes = await _dashboardRepository.GetDashboardIncomeAsync(user?.Name ?? "ERROR");

            // Map the chart4 data
            var rawRecords = dashboardChart4.Select(c => new DashboardChart4Record
            {
                Key = c.Key,
                Name = c.Name,
                Account = c.Account,
                IntAmount = c.IntAmount,
                Int_amount_char = c.Int_amount_char
            }).ToList();

            // Calculate account balances
            var accountBalances = rawRecords
                .GroupBy(r => r.Account)
                .Select(g => new DashboardChart4AccountBalance
                {
                    Account_name = g.Key,
                    Account_balance = g.Sum(x => x.IntAmount ?? 0)
                }).ToList();

            // Combine the records and account balances into a single array
            var chart1 = new List<object>
            {
                dashboardChart1
            };

            var chart2 = new List<object>
            {
                dashboardChart2
            };

            var chart3 = new List<object>
            {
               dashboardChart3
            };

            var chart4 = new List<object>
            {
                rawRecords, // first element: list of records
                accountBalances // second element: list of account balances
            };

            var ChartExpenses = new List<object>
            {
                dashboardExpenses
            };

            var ChartIncomes = new List<object>
            {
                dashboardIncomes
            };

            return new DashboardResponse
            {
                Chart1 = chart1,
                Chart2 = chart2,
                Chart3 = chart3,
                Chart4 = chart4,
                ChartExpenses = ChartExpenses,
                ChartIncomes = ChartIncomes

            };
        }
    }
}
