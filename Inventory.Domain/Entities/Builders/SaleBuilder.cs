namespace Inventory.Domain.Entities
{
    public class SaleBuilder
    {
        private readonly Sale _sale = new();

        public SaleBuilder WithId(Guid id)
        {
            _sale.Id = id;
            return this;
        }

        public SaleBuilder WithDate(DateTime date)
        {
            _sale.Date = date;
            return this;
        }

        public SaleBuilder WithTotal(double total)
        {
            _sale.Total = total;
            return this;
        }

        public SaleBuilder WithBranchId(Guid branchId)
        {
            _sale.BranchId = branchId;
            return this;
        }

        public SaleBuilder WithBranch(Branch branch)
        {
            _sale.Branch = branch;
            return this;
        }

        public SaleBuilder WithSellerId(Guid sellerId)
        {
            _sale.SellerId = sellerId;
            return this;
        }

        public SaleBuilder WithSeller(User seller)
        {
            _sale.Seller = seller;
            return this;
        }

        public SaleBuilder WithCustomerId(Guid? customerId)
        {
            _sale.CustomerId = customerId;
            return this;
        }

        public SaleBuilder WithCustomer(Customer customer)
        {
            _sale.Customer = customer;
            return this;
        }

        public SaleBuilder WithSaleDetails(ICollection<SaleDetail> saleDetails)
        {
            _sale.SaleDetails = saleDetails;
            return this;
        }

        public SaleBuilder AddSaleDetail(SaleDetail saleDetail)
        {
            _sale.SaleDetails.Add(saleDetail);
            return this;
        }

        public Sale Build() => _sale;
    }
}