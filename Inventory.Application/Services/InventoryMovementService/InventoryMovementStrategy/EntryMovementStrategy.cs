using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.DataTransferObjects.InventoryMovementDto;
using Inventory.Domain.Entities;
using Inventory.Domain.Enum;

namespace Inventory.Application.Services.InventoryMovementService.InventoryMovementStrategy
{
    public class EntryMovementStrategy(IBranchRepository branchRepository, IWarehouseRepository warehouseRepository) : IInventoryMovementStrategy
    {
        public EnumMovementType Type => EnumMovementType.Entry;

        public async Task<InventoryMovement> ExecuteAsync(InventoryMovementRequest request, IInventoryMovementRepository repository, IMapper mapper, Guid user)
        {
            var toWarehouse = request.ToWarehouseId.HasValue ? await FindWarehouseProductByWarehouseIdAndProductIdAsync(request.ToWarehouseId, request.ProductId) : null;
            var toBranch = request.ToBranchId.HasValue ? await FindBranchProductByBranchIdAndProductIdAsync(request.ToBranchId, request.ProductId) : null;
            toWarehouse?.Stock += request.Quantity;
            toBranch?.Stock += request.Quantity;
            var inventoryMovement = mapper.Map<InventoryMovement>(request);
            inventoryMovement.UserId = user;
            var auditHistory = new AuditHistoryBuilder()
                .WithUserId(user)
                .WithAction(EnumAction.Create)
                .WithEntity(EnumEntity.InventoryMovement)
                .WithCreatedAt(DateTime.UtcNow)
                .WithDescription($"Created entry movement for product {toBranch?.Product.Name ?? toWarehouse?.Product.Name} with quantity {request.Quantity}.")
                .Build();
            return await repository.CreateInventoryMovementAsync(inventoryMovement, toWarehouse, toBranch, auditHistory);
        }

        private async Task<WarehouseProduct> FindWarehouseProductByWarehouseIdAndProductIdAsync(Guid? warehouseId, int productId)
        {
            return await warehouseRepository.GetWarehouseProductByWarehouseIdAndProductIdAsync(warehouseId, productId) ?? throw new InvalidOperationException("Warehouse product not found.");
        }

        private async Task<BranchProduct> FindBranchProductByBranchIdAndProductIdAsync(Guid? branchId, int productId)
        {
            return await branchRepository.GetBranchProductByBranchIdAndProductIdAsync(branchId, productId) ?? throw new InvalidOperationException("Branch product not found.");
        }
    }
}
