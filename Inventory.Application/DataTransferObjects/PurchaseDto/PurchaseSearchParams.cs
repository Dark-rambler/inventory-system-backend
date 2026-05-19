namespace Inventory.Application.DataTransferObjects.PurchaseDto
{
    public class PurchaseSearchParams
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Guid? ProviderId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? WarehouseId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}