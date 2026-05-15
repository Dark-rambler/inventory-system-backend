namespace Inventory.Domain.Entities.Builders
{
    public class BranchProductBuilder
    {
        private readonly BranchProduct _branchProduct = new();
        public BranchProductBuilder WithBranchId(Guid branchId)
        {
            _branchProduct.BranchId = branchId;
            return this;
        }
        public BranchProductBuilder WithProductId(int productId)
        {
            _branchProduct.ProductId = productId;
            return this;
        }
        public BranchProductBuilder WithStock(int stock)
        {
            _branchProduct.Stock = stock;
            return this;
        }
        public BranchProductBuilder WithPrice(double price)
        {
            _branchProduct.Price = price;
            return this;
        }
        public BranchProductBuilder WithLowStock(int lowStock)
        {
            _branchProduct.LowStock = lowStock;
            return this;
        }
        public BranchProduct Build() => _branchProduct;
    }
}
