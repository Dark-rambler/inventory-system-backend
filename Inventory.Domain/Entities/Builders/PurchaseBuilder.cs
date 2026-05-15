namespace Inventory.Domain.Entities
{
    public class PurchaseBuilder
    {
        private readonly Purchase _purchase = new();

        public PurchaseBuilder WithId(Guid id)
        {
            _purchase.Id = id;
            return this;
        }

        public PurchaseBuilder WithBusinessId(Guid businessId)
        {
            _purchase.BusinessId = businessId;
            return this;
        }

        public PurchaseBuilder WithDate(DateTime date)
        {
            _purchase.Date = date;
            return this;
        }

        public PurchaseBuilder WithTotal(double total)
        {
            _purchase.Total = total;
            return this;
        }

        public PurchaseBuilder WithProviderId(Guid providerId)
        {
            _purchase.ProviderId = providerId;
            return this;
        }

        public PurchaseBuilder WithProvider(Provider provider)
        {
            _purchase.Provider = provider;
            return this;
        }

        public PurchaseBuilder WithBuyerId(Guid buyerId)
        {
            _purchase.BuyerId = buyerId;
            return this;
        }

        public PurchaseBuilder WithBuyer(User buyer)
        {
            _purchase.Buyer = buyer;
            return this;
        }

        public PurchaseBuilder WithBranchId(Guid? branchId)
        {
            _purchase.BranchId = branchId;
            return this;
        }

        public PurchaseBuilder WithBranch(Branch? branch)
        {
            _purchase.Branch = branch;
            return this;
        }

        public PurchaseBuilder WithWarehouseId(Guid? warehouseId)
        {
            _purchase.WarehouseId = warehouseId;
            return this;
        }

        public PurchaseBuilder WithWarehouse(Warehouse? warehouse)
        {
            _purchase.Warehouse = warehouse;
            return this;
        }

        public PurchaseBuilder WithPurchaseDetails(ICollection<PurchaseDetail> purchaseDetails)
        {
            _purchase.PurchaseDetails = purchaseDetails;
            return this;
        }

        public PurchaseBuilder AddPurchaseDetail(PurchaseDetail purchaseDetail)
        {
            _purchase.PurchaseDetails.Add(purchaseDetail);
            return this;
        }

        public Purchase Build() => _purchase;
    }
}