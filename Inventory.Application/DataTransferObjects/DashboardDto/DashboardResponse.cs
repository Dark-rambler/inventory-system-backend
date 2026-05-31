namespace Inventory.Application.DataTransferObjects.DashboardDto
{
    public class DashboardResponse
    {
        public int TodaySalesCount { get; set; }
        public double TodaySalesTotal { get; set; }
        public int TodayMovementsCount { get; set; }
        public int LowStockProductsCount { get; set; }
    }
}
