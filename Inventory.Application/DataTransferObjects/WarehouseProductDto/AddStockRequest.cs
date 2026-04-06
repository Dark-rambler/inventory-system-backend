namespace Inventory.Application.DataTransferObjects.WarehouseProductDto
{
    public record AddStockRequest(Guid ProductId, int Stock);
}
