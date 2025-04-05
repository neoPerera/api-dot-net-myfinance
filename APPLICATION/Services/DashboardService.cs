using APPLICATION.DTOs;
using APPLICATION.Interfaces;
using CORE.Interfaces;
using Newtonsoft.Json;

namespace APPLICATION.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardResponse> GetDashboardDataAsync(string userId)
        {
            // Fetch the data from the repository (returning the list of entities)
            var dashboardChart1 = await _dashboardRepository.GetDashboardChart1DataAsync(userId);
            var dashboardChart2 = await _dashboardRepository.GetDashboardChart2DataAsync(userId);
            var dashboardChart3 = await _dashboardRepository.GetDashboardChart3DataAsync(userId);

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

            return new DashboardResponse
            {
                Chart1 = Mappedchart1,
                Chart2 = Mappedchart2,
                Chart3 = Mappedchart3

            };
        }
    }
}
