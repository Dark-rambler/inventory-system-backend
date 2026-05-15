namespace Inventory.Application.DataTransferObjects.PurchaseDto
{
    public class PurchaseRequest
    {
        public Guid ProviderId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? WarehouseId { get; set; }
        public IEnumerable<PurchaseDetailRequest> PurchaseDetails { get; set; } = [];
    }
}