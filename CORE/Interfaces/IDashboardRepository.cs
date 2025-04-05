using CORE.Entities;

namespace CORE.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<DashboardChart1>> GetDashboardChart1DataAsync(string userId);
        Task<List<DashboardChart2>> GetDashboardChart2DataAsync(string userId);
        Task<List<DashboardChart3>> GetDashboardChart3DataAsync(string userId);
        Task<List<DashboardChart4>> GetDashboardChart4DataAsync(string userId);




    }
}
