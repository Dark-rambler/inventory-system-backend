namespace Inventory.Domain.Entities.Builders
{
    public class WarehouseProductBuilder
    {
        private readonly WarehouseProduct _warehouseProduct = new();
        public WarehouseProductBuilder WithWarehouseId(Guid warehouseId)
        {
            _warehouseProduct.WarehouseId = warehouseId;
            return this;
        }
        public WarehouseProductBuilder WithProductId(Guid productId)
        {
            _warehouseProduct.ProductId = productId;
            return this;
        }
        public WarehouseProductBuilder WithStock(int stock)
        {
            _warehouseProduct.Stock = stock;
            return this;
        }
        public WarehouseProductBuilder WithLowStock(int lowStock)
        {
            _warehouseProduct.LowStock = lowStock;
            return this;
        }
        public WarehouseProduct Build() => _warehouseProduct;
    }
}
