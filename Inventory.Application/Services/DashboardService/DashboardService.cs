using Inventory.Application.Common.Abstracts;
using Inventory.Application.DataTransferObjects.DashboardDto;

namespace Inventory.Application.Services.DashboardService
{
    public class DashboardService(IDashboardRepository repository, IDateTimeProvider dateTimeProvider) : IDashboardService
    {
        public async Task<DashboardResponse> GetTodayStatsAsync(Guid businessId, Guid? branchId)
        {
            var start = dateTimeProvider.UtcNow.Date;
            var end = start.AddDays(1);
            return new DashboardResponse
            {
                TodaySalesCount = await repository.GetTodaySalesCountAsync(businessId, branchId, start, end),
                TodaySalesTotal = await repository.GetTodaySalesTotalAsync(businessId, branchId, start, end),
                TodayMovementsCount = await repository.GetTodayMovementsCountAsync(businessId, branchId, start, end),
                LowStockProductsCount = await repository.GetLowStockProductsCountAsync(businessId, branchId)
            };
        }
    }
}
