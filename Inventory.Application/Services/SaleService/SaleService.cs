using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BranchProductDto;
using Inventory.Domain.Entities;
using Inventory.Domain.Entities.Builders;
using Inventory.Domain.Enum;

namespace Inventory.Application.Services.SaleService
{
    public class SaleService(
        IBranchRepository repository,
        IMapper mapper,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider,
        IBusinessSaleCounterRepository saleCounterRepository) : ISaleService
    {
        public async Task CreateSaleAsync(Guid id, SaleRequest request, Guid businessId)
        {
            await FindBranchById(id, businessId);
            var user = currentUserService.GetCurrentUserId();
            var productIds = request.SaleDetails.Select(sd => sd.ProductId).ToList();
            var products = await repository.GetBranchProductsByProductIdsAsync(id, productIds);
            var createdAt = dateTimeProvider.UtcNow;

            var productsUpdated = request.SaleDetails.Select(sd =>
            {
                var product = products.First(p => p.BranchId == id && p.ProductId == sd.ProductId);
                product.ReduceStock(sd.Quantity);
                return product;
            }).ToList();

            var folio = await saleCounterRepository.GetNextFolioAsync(businessId);

            var sale = new SaleBuilder()
                .WithBusinessId(businessId)
                .WithBranchId(id)
                .WithSellerId(user)
                .WithCustomerId(request.CustomerId)
                .WithFolio(folio)
                .WithDate(createdAt)
                .WithTotal(request.SaleDetails.Sum(sd => sd.Quantity * products.First(p => p.BranchId == id && p.ProductId == sd.ProductId).Price))
                .WithSaleDetails([.. request.SaleDetails.Select(sd => new SaleDetailBuilder()
                    .WithProductId(sd.ProductId)
                    .WithQuantity(sd.Quantity)
                    .WithPrice(products.First(p => p.BranchId == id && p.ProductId == sd.ProductId).Price)
                    .Build())])
                .Build();

            var inventoryMovements = request.SaleDetails.Select(sd => new InventoryMovementBuilder()
                .WithProductId(sd.ProductId)
                .WithBusinessId(businessId)
                .WithFromBranchId(id)
                .WithQuantity(sd.Quantity)
                .WithType(EnumMovementType.Exit)
                .WithIsSale(true)
                .WithUserId(user)
                .WithCreatedAt(createdAt)
                .Build()
            ).ToList();

            var auditHistory = new AuditHistoryBuilder()
                .WithAction(EnumAction.Create)
                .WithEntity(EnumEntity.Sale)
                .WithUserId(user)
                .WithBusinessId(businessId)
                .WithCreatedAt(createdAt)
                .WithDescription($"Sale created with total {sale.Total}")
                .Build();

            await repository.CreateSaleAsync(sale, inventoryMovements, productsUpdated, auditHistory);
        }

        public async Task<PaginatedList<SaleResponse>> GetSalesByBranchAsync(Guid id, SaleSearchParams searchParams, Guid businessId)
        {
            await FindBranchById(id, businessId);
            var paginatedSales = await repository.GetSalesByBranchAsync(businessId, id, searchParams.FromDate, searchParams.ToDate, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<SaleResponse>(
                mapper.Map<List<SaleResponse>>(paginatedSales.Items),
                paginatedSales.TotalCount,
                paginatedSales.PageIndex,
                paginatedSales.PageSize
            );
        }

        private async Task<Branch> FindBranchById(Guid id, Guid businessId)
        {
            return await repository.GetBranchByIdAsync(id, businessId) ?? throw new KeyNotFoundException($"Branch with id {id} doesn't exist");
        }
    }
}
