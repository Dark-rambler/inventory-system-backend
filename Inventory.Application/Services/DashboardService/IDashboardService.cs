using Inventory.Application.DataTransferObjects.DashboardDto;

namespace Inventory.Application.Services.DashboardService
{
    public interface IDashboardService
    {
        Task<DashboardResponse> GetTodayStatsAsync(Guid businessId, Guid? branchId);
    }
}
