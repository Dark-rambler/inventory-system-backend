using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.DataTransferObjects.InventoryMovementDto;
using Inventory.Domain.Entities;
using Inventory.Domain.Enum;

namespace Inventory.Application.Services.InventoryMovementService.InventoryMovementStrategy
{
    public class TransferMovementStrategy(IBranchRepository branchRepository, IWarehouseRepository warehouseRepository) : IInventoryMovementStrategy
    {
        public MovementType Type => MovementType.Transfer;

        public async Task<InventoryMovement> ExecuteAsync(InventoryMovementRequest request, IInventoryMovementRepository repository, IMapper mapper, Guid user)
        {
            var toWarehouse = request.ToWarehouseId.HasValue ? await FindWarehouseProductByWarehouseIdAndProductIdAsync(request.ToWarehouseId, request.ProductId) : null;
            var toBranch = request.ToBranchId.HasValue ? await FindBranchProductByBranchIdAndProductIdAsync(request.ToBranchId, request.ProductId) : null;
            var fromWarehouse = request.FromWarehouseId.HasValue ? await FindWarehouseProductByWarehouseIdAndProductIdAsync(request.FromWarehouseId, request.ProductId) : null;
            var fromBranch = request.FromBranchId.HasValue ? await FindBranchProductByBranchIdAndProductIdAsync(request.FromBranchId, request.ProductId) : null;
            toWarehouse?.Stock += request.Quantity;
            toBranch?.Stock += request.Quantity;
            if (fromWarehouse != null)
            {
                if (fromWarehouse.Stock > request.Quantity)
                    fromWarehouse.Stock -= request.Quantity;
                else
                    throw new InvalidOperationException("Insufficient stock for the exit movement.");
            }
            if (fromBranch != null)
            {
                if (fromBranch.Stock > request.Quantity)
                    fromBranch.Stock -= request.Quantity;
                else
                    throw new InvalidOperationException("Insufficient stock for the exit movement.");
            }
            var inventoryMovement = mapper.Map<InventoryMovement>(request);
            inventoryMovement.UserId = user;
            return await repository.CreateInventoryMovementAsync(inventoryMovement, fromWarehouse ?? toWarehouse, fromBranch ?? toBranch);
        }

        private async Task<WarehouseProduct> FindWarehouseProductByWarehouseIdAndProductIdAsync(Guid? warehouseId, Guid productId)
        {
            return await warehouseRepository.GetWarehouseProductByWarehouseIdAndProductIdAsync(warehouseId, productId) ?? throw new KeyNotFoundException("Warehouse product not found.");
        }

        private async Task<BranchProduct> FindBranchProductByBranchIdAndProductIdAsync(Guid? branchId, Guid productId)
        {
            return await branchRepository.GetBranchProductByBranchIdAndProductIdAsync(branchId, productId) ?? throw new KeyNotFoundException("Branch product not found.");
        }
    }
}
