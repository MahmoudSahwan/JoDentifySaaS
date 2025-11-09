using JoDentify.Application.DTOs.Dashboard;

namespace JoDentify.Application.Interfaces
{
    public interface IDashboardService
    {
       
        Task<DashboardStatsDto> GetDashboardStatsAsync();
    }
}