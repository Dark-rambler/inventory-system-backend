using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.DataTransferObjects.InventoryMovementDto;
using Inventory.Domain.Entities;
using Inventory.Domain.Enum;

namespace Inventory.Application.Services.InventoryMovementService.InventoryMovementStrategy
{
    public class ExitMovementStrategy(
        IBranchRepository branchRepository,
        IWarehouseRepository warehouseRepository,
        IInventoryMovementRepository repository,
        IMapper mapper,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider) : IInventoryMovementStrategy
    {
        public EnumMovementType Type => EnumMovementType.Exit;

        public async Task<InventoryMovement> ExecuteAsync(InventoryMovementRequest request)
        {
            var user = currentUserService.GetCurrentUserId();
            var fromWarehouse = request.FromWarehouseId.HasValue ? await FindWarehouseProductAsync(request.FromWarehouseId, request.ProductId) : null;
            var fromBranch = request.FromBranchId.HasValue ? await FindBranchProductAsync(request.FromBranchId, request.ProductId) : null;
            fromWarehouse?.ReduceStock(request.Quantity);
            fromBranch?.ReduceStock(request.Quantity);
            var inventoryMovement = mapper.Map<InventoryMovement>(request);
            inventoryMovement.UserId = user;
            var auditHistory = new AuditHistoryBuilder()
                .WithUserId(user)
                .WithAction(EnumAction.Create)
                .WithEntity(EnumEntity.InventoryMovement)
                .WithCreatedAt(dateTimeProvider.UtcNow)
                .WithDescription($"Created exit movement for product {fromBranch?.Product.Name ?? fromWarehouse?.Product.Name} with quantity {request.Quantity}.")
                .Build();
            return await repository.CreateInventoryMovementAsync(inventoryMovement, fromWarehouse, fromBranch, auditHistory);
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
