using APPLICATION.DTOs;

namespace APPLICATION.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardResponse> GetDashboardDataAsync(string userId);
    }
}
