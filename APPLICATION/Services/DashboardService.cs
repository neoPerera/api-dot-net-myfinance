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
            var dashboardChart1 = await _dashboardRepository.GetDashboardChart1DataAsync(user?.Name ?? "test");
            var dashboardChart2 = await _dashboardRepository.GetDashboardChart2DataAsync(user?.Name ?? "test");
            var dashboardChart3 = await _dashboardRepository.GetDashboardChart3DataAsync(user?.Name ?? "test");
            var dashboardChart4 = await _dashboardRepository.GetDashboardChart4DataAsync(user?.Name ?? "test");


            var Mappedchart1 = dashboardChart1.Select(c => new DashboardChart1DTO
            {
                Type = c.Type ?? "No Type",    // Handle null if any
                Value = c.Value != null ? c.Value : 0 // Default to 0 if null
            }).ToList();

            var Mappedchart2 = dashboardChart2.Select(c => new DashboardChart2DTO
            {
                Source1 = c.Source1 ?? "No Type",    // Handle null if any
                Target = c.Target ?? "No Target",    // Handle null if any
                Value = c.Value != null ? c.Value : 0 // Default to 0 if null
            }).ToList();
            var Mappedchart3 = dashboardChart3.Select(c => new DashboardChart3DTO
            {
                Type = c.Type ?? "No Type",    // Handle null if any
                Value = c.Value != null ? c.Value : 0 // Default to 0 if null
            }).ToList();

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
                Mappedchart1
            };

            var chart2 = new List<object>
            {
                Mappedchart2
            };

            var chart3 = new List<object>
            {
               Mappedchart3
            };

            var chart4 = new List<object>
            {
                rawRecords, // first element: list of records
                accountBalances // second element: list of account balances
            };

            return new DashboardResponse
            {
                Chart1 = chart1,
                Chart2 = chart2,
                Chart3 = chart3,
                Chart4 = chart4

            };
        }
    }
}
