using AutoMapper;
using FluentValidation;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.PurchaseDto;
using Inventory.Domain.Entities;
using Inventory.Domain.Enum;

namespace Inventory.Application.Services.PurchaseService
{
    public class PurchaseService(
        IPurchaseRepository repository,
        IMapper mapper,
        IValidator<PurchaseRequest> validator,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider) : IPurchaseService
    {
        public async Task CreatePurchaseAsync(PurchaseRequest request, Guid businessId)
        {
            await validator.ValidateAndThrowAsync(request);
            var user = currentUserService.GetCurrentUserId();

            var productIds = request.PurchaseDetails.Select(pd => pd.ProductId).ToList();
            List<BranchProduct>? productsByBranchUpdated = null;
            if (request.BranchId.HasValue)
            {
                var productsByBranch = await repository.GetBranchProductsByProductIdsAsync(request.BranchId.Value, productIds);
                productsByBranchUpdated = [.. request.PurchaseDetails.Select(pd =>
                {
                    var product = productsByBranch.First(p => p.ProductId == pd.ProductId);
                    product.AddStock(pd.Quantity);
                    return product;
                })];
            }

            List<WarehouseProduct>? productsByWarehouseUpdated = null;
            if (request.WarehouseId.HasValue)
            {
                var productsByWarehouse = await repository.GetWarehouseProductsByProductIdsAsync(request.WarehouseId.Value, productIds);
                productsByWarehouseUpdated = [.. request.PurchaseDetails.Select(pd =>
                {
                    var product = productsByWarehouse.First(p => p.ProductId == pd.ProductId);
                    product.AddStock(pd.Quantity);
                    return product;
                })];
            }

            var createdAt = dateTimeProvider.UtcNow;

            var purchase = new PurchaseBuilder()
                .WithBusinessId(businessId)
                .WithProviderId(request.ProviderId)
                .WithBuyerId(user)
                .WithBranchId(request.BranchId)
                .WithWarehouseId(request.WarehouseId)
                .WithDate(createdAt)
                .WithTotal(request.PurchaseDetails.Sum(pd => pd.Quantity * pd.Price))
                .WithPurchaseDetails([.. request.PurchaseDetails.Select(pd => new PurchaseDetailBuilder()
                    .WithProductId(pd.ProductId)
                    .WithQuantity(pd.Quantity)
                    .WithPrice(pd.Price)
                    .Build())])
                .Build();

            var inventoryMovements = request.PurchaseDetails.Select(pd => new InventoryMovementBuilder()
                .WithProductId(pd.ProductId)
                .WithQuantity(pd.Quantity)
                .WithType(EnumMovementType.Entry)
                .WithIsPurchase(true)
                .WithToBranchId(request.BranchId)
                .WithToWarehouseId(request.WarehouseId)
                .WithUserId(user)
                .WithCreatedAt(createdAt)
                .Build()
            ).ToList();

            var auditHistory = new AuditHistoryBuilder()
                .WithAction(EnumAction.Create)
                .WithEntity(EnumEntity.Purchase)
                .WithUserId(user)
                .WithBusinessId(businessId)
                .WithCreatedAt(createdAt)
                .WithDescription($"Purchase created with total {purchase.Total}")
                .Build();

            await repository.CreatePurchaseAsync(purchase, inventoryMovements, productsByBranchUpdated, productsByWarehouseUpdated, auditHistory);
        }

        public async Task<PaginatedList<PurchaseResponse>> GetPurchasesAsync(PurchaseSearchParams searchParams, Guid businessId)
        {
            var paginatedPurchases = await repository.GetPurchasesAsync(
                businessId,
                searchParams.FromDate,
                searchParams.ToDate,
                searchParams.ProviderId,
                searchParams.BranchId,
                searchParams.Page,
                searchParams.PageSize);

            return new PaginatedList<PurchaseResponse>(
                mapper.Map<List<PurchaseResponse>>(paginatedPurchases.Items),
                paginatedPurchases.TotalCount,
                paginatedPurchases.PageIndex,
                paginatedPurchases.PageSize
            );
        }
    }
}
