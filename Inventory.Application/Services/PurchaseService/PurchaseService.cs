using AutoMapper;
using FluentValidation;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.Common.Utils;
using Inventory.Application.DataTransferObjects.PurchaseDto;
using Inventory.Domain.Entities;
using Inventory.Domain.Enum;

namespace Inventory.Application.Services.PurchaseService
{
    public class PurchaseService(
        IPurchaseRepository repository,
        IMapper mapper,
        IValidator<PurchaseRequest> validator) : IPurchaseService
    {
        public async Task CreatePurchaseAsync(PurchaseRequest request, Guid user)
        {
            await validator.ValidateAndThrowAsync(request);

            var productIds = request.PurchaseDetails.Select(pd => pd.ProductId).ToList();
            var products = await repository.GetBranchProductsByProductIdsAsync(request.BranchId, productIds);
            var createdAt = DateTime.UtcNow;

            var productsUpdated = request.PurchaseDetails.Select(pd =>
            {
                var product = products.First(p => p.ProductId == pd.ProductId);
                StockUtil.AddStock(product, pd.Quantity);
                return product;
            }).ToList();

            var purchase = new PurchaseBuilder()
                .WithProviderId(request.ProviderId)
                .WithBuyerId(user)
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
                .WithUserId(user)
                .WithCreatedAt(createdAt)
                .Build()
            ).ToList();

            var auditHistory = new AuditHistoryBuilder()
                .WithAction(EnumAction.Create)
                .WithEntity(EnumEntity.Purchase)
                .WithUserId(user)
                .WithCreatedAt(createdAt)
                .WithDescription($"Purchase created with total {purchase.Total}")
                .Build();

            await repository.CreatePurchaseAsync(purchase, inventoryMovements, productsUpdated, auditHistory);
        }

        public async Task<PaginatedList<PurchaseResponse>> GetPurchasesAsync(PurchaseSearchParams searchParams)
        {
            var paginatedPurchases = await repository.GetPurchasesAsync(
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