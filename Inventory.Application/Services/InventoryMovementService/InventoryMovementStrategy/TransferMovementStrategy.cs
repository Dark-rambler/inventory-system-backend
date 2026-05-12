using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.DataTransferObjects.InventoryMovementDto;
using Inventory.Domain.Entities;
using Inventory.Domain.Enum;

namespace Inventory.Application.Services.InventoryMovementService.InventoryMovementStrategy
{
    public class TransferMovementStrategy(
        IBranchRepository branchRepository,
        IWarehouseRepository warehouseRepository,
        IInventoryMovementRepository repository,
        IMapper mapper,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider) : IInventoryMovementStrategy
    {
        public EnumMovementType Type => EnumMovementType.Transfer;

        public async Task<InventoryMovement> ExecuteAsync(InventoryMovementRequest request)
        {
            var user = currentUserService.GetCurrentUserId();
            var toWarehouse = request.ToWarehouseId.HasValue ? await FindWarehouseProductAsync(request.ToWarehouseId, request.ProductId) : null;
            var toBranch = request.ToBranchId.HasValue ? await FindBranchProductAsync(request.ToBranchId, request.ProductId) : null;
            var fromWarehouse = request.FromWarehouseId.HasValue ? await FindWarehouseProductAsync(request.FromWarehouseId, request.ProductId) : null;
            var fromBranch = request.FromBranchId.HasValue ? await FindBranchProductAsync(request.FromBranchId, request.ProductId) : null;
            toWarehouse?.AddStock(request.Quantity);
            toBranch?.AddStock(request.Quantity);
            fromWarehouse?.ReduceStock(request.Quantity);
            fromBranch?.ReduceStock(request.Quantity);
            var inventoryMovement = mapper.Map<InventoryMovement>(request);
            inventoryMovement.UserId = user;
            var auditHistory = new AuditHistoryBuilder()
                .WithUserId(user)
                .WithAction(EnumAction.Create)
                .WithEntity(EnumEntity.InventoryMovement)
                .WithCreatedAt(dateTimeProvider.UtcNow)
                .WithDescription($"Created transfer movement for product {fromWarehouse?.Product.Name ?? fromBranch?.Product.Name} with quantity {request.Quantity}.")
                .Build();
            return await repository.CreateInventoryMovementAsync(inventoryMovement, fromWarehouse ?? toWarehouse, fromBranch ?? toBranch, auditHistory);
        }

        private async Task<WarehouseProduct> FindWarehouseProductAsync(Guid? warehouseId, int productId)
        {
            return await warehouseRepository.GetWarehouseProductByWarehouseIdAndProductIdAsync(warehouseId, productId)
                ?? throw new KeyNotFoundException("Warehouse product not found.");
        }

        private async Task<BranchProduct> FindBranchProductAsync(Guid? branchId, int productId)
        {
            return await branchRepository.GetBranchProductByBranchIdAndProductIdAsync(branchId, productId)
                ?? throw new KeyNotFoundException("Branch product not found.");
        }
    }
}
