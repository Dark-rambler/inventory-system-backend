namespace Inventory.Application.DataTransferObjects.BranchProductDto
{
    public record AddStockToBranchRequest(Guid ProductId, Guid WarehouseId, int Stock);
}
