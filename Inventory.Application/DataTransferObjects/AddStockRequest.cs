namespace Inventory.Application.DataTransferObjects
{
    public record AddStockRequest(Guid ProductId, int Stock);
}
