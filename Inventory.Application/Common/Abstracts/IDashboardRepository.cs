namespace Inventory.Application.Common.Abstracts
{
    public interface IDashboardRepository
    {
        Task<int> GetTodaySalesCountAsync(Guid businessId, Guid? branchId, DateTime start, DateTime end);
        Task<double> GetTodaySalesTotalAsync(Guid businessId, Guid? branchId, DateTime start, DateTime end);
        Task<int> GetTodayMovementsCountAsync(Guid businessId, Guid? branchId, DateTime start, DateTime end);
        Task<int> GetLowStockProductsCountAsync(Guid businessId, Guid? branchId);
    }
}
