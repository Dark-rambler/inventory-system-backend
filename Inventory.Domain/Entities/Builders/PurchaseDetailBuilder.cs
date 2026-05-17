namespace Inventory.Domain.Entities.Builders
{
    public class PurchaseDetailBuilder
    {
        private readonly PurchaseDetail _purchaseDetail = new();

        public PurchaseDetailBuilder WithId(int id)
        {
            _purchaseDetail.Id = id;
            return this;
        }

        public PurchaseDetailBuilder WithQuantity(int quantity)
        {
            _purchaseDetail.Quantity = quantity;
            return this;
        }

        public PurchaseDetailBuilder WithPrice(double price)
        {
            _purchaseDetail.Price = price;
            return this;
        }

        public PurchaseDetailBuilder WithPurchaseId(Guid purchaseId)
        {
            _purchaseDetail.PurchaseId = purchaseId;
            return this;
        }

        public PurchaseDetailBuilder WithPurchase(Purchase purchase)
        {
            _purchaseDetail.Purchase = purchase;
            return this;
        }

        public PurchaseDetailBuilder WithProductId(int productId)
        {
            _purchaseDetail.ProductId = productId;
            return this;
        }

        public PurchaseDetailBuilder WithProduct(Product product)
        {
            _purchaseDetail.Product = product;
            return this;
        }

        public PurchaseDetail Build() => _purchaseDetail;
    }
}