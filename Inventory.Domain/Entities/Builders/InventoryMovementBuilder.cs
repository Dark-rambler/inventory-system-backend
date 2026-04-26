using Inventory.Domain.Enum;

namespace Inventory.Domain.Entities
{
    public class InventoryMovementBuilder
    {
        private readonly InventoryMovement _inventoryMovement = new();

        public InventoryMovementBuilder WithId(Guid id)
        {
            _inventoryMovement.Id = id;
            return this;
        }

        public InventoryMovementBuilder WithDescription(string description)
        {
            _inventoryMovement.Description = description;
            return this;
        }

        public InventoryMovementBuilder WithProductId(Guid productId)
        {
            _inventoryMovement.ProductId = productId;
            return this;
        }

        public InventoryMovementBuilder WithProduct(Product product)
        {
            _inventoryMovement.Product = product;
            return this;
        }

        public InventoryMovementBuilder WithQuantity(int quantity)
        {
            _inventoryMovement.Quantity = quantity;
            return this;
        }

        public InventoryMovementBuilder WithType(EnumMovementType type)
        {
            _inventoryMovement.Type = type;
            return this;
        }

        public InventoryMovementBuilder WithIsSale(bool isSale)
        {
            _inventoryMovement.IsSale = isSale;
            return this;
        }

        public InventoryMovementBuilder WithIsPurchase(bool isPurchase)
        {
            _inventoryMovement.IsPurchase = isPurchase;
            return this;
        }

        public InventoryMovementBuilder WithFromWarehouseId(Guid? fromWarehouseId)
        {
            _inventoryMovement.FromWarehouseId = fromWarehouseId;
            return this;
        }

        public InventoryMovementBuilder WithFromWarehouse(Warehouse? fromWarehouse)
        {
            _inventoryMovement.FromWarehouse = fromWarehouse;
            return this;
        }

        public InventoryMovementBuilder WithFromBranchId(Guid? fromBranchId)
        {
            _inventoryMovement.FromBranchId = fromBranchId;
            return this;
        }

        public InventoryMovementBuilder WithFromBranch(Branch? fromBranch)
        {
            _inventoryMovement.FromBranch = fromBranch;
            return this;
        }

        public InventoryMovementBuilder WithToWarehouseId(Guid? toWarehouseId)
        {
            _inventoryMovement.ToWarehouseId = toWarehouseId;
            return this;
        }

        public InventoryMovementBuilder WithToWarehouse(Warehouse? toWarehouse)
        {
            _inventoryMovement.ToWarehouse = toWarehouse;
            return this;
        }

        public InventoryMovementBuilder WithToBranchId(Guid? toBranchId)
        {
            _inventoryMovement.ToBranchId = toBranchId;
            return this;
        }

        public InventoryMovementBuilder WithToBranch(Branch? toBranch)
        {
            _inventoryMovement.ToBranch = toBranch;
            return this;
        }

        public InventoryMovementBuilder WithUserId(Guid userId)
        {
            _inventoryMovement.UserId = userId;
            return this;
        }

        public InventoryMovementBuilder WithUser(User user)
        {
            _inventoryMovement.User = user;
            return this;
        }

        public InventoryMovementBuilder WithCreatedAt(DateTime createdAt)
        {
            _inventoryMovement.CreatedAt = createdAt;
            return this;
        }

        public InventoryMovement Build() => _inventoryMovement;
    }
}