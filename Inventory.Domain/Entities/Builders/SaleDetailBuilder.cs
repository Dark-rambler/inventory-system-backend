namespace Inventory.Domain.Entities
{
    public class SaleDetailBuilder
    {
        private readonly SaleDetail _saleDetail = new();

        public SaleDetailBuilder WithId(int id)
        {
            _saleDetail.Id = id;
            return this;
        }

        public SaleDetailBuilder WithQuantity(int quantity)
        {
            _saleDetail.Quantity = quantity;
            return this;
        }

        public SaleDetailBuilder WithPrice(double price)
        {
            _saleDetail.Price = price;
            return this;
        }

        public SaleDetailBuilder WithSaleId(Guid saleId)
        {
            _saleDetail.SaleId = saleId;
            return this;
        }

        public SaleDetailBuilder WithSale(Sale sale)
        {
            _saleDetail.Sale = sale;
            return this;
        }

        public SaleDetailBuilder WithProductId(int productId)
        {
            _saleDetail.ProductId = productId;
            return this;
        }

        public SaleDetailBuilder WithProduct(Product product)
        {
            _saleDetail.Product = product;
            return this;
        }

        public SaleDetail Build() => _saleDetail;
    }
}