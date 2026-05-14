using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Context;
using Inventory.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Inventory.Infrastructure.Repositories
{
    public class BranchRepository(InventoryDbContext context, IDateTimeProvider dateTimeProvider) : IBranchRepository
    {
        public async Task<PaginatedList<Branch>> GetBranchesAsync(Guid businessId, string? name, int page, int pageSize) =>
            await context.Branches
                .AsQueryable()
                .Where(b => b.BusinessId == businessId)
                .Include(b => b.Location)
                .OrderByDescending(b => b.CreatedAt)
                .FiltersBranch(name)
                .ToPaginatedListAsync(page, pageSize);

        public async Task<Branch?> GetBranchByIdAsync(Guid id) =>
            await context.Branches
                .Include(w => w.Location)
                .FirstOrDefaultAsync(b => b.Id == id);

        public async Task<Branch> CreateBranchAsync(Branch branch)
        {
            context.Branches.Add(branch);
            await context.SaveChangesAsync();
            return await context.Branches
                .Include(w => w.Location)
                .FirstAsync(b => b.Id == branch.Id);
        }

        public async Task UpdateBranchAsync(Branch branch)
        {
            branch.UpdatedAt = dateTimeProvider.UtcNow;
            context.Branches.Update(branch);
            await context.SaveChangesAsync();
        }

        public async Task DeleteBranchAsync(Branch branch)
        {
            branch.IsDeleted = true;
            await context.SaveChangesAsync();
        }

        public async Task<PaginatedList<BranchProduct>> GetProductsByBranchAsync(Guid id, string? name, int page, int pageSize) =>
            await context.BranchProducts
                .AsQueryable()
                .Where(bp => bp.BranchId == id)
                .Include(bp => bp.Product)
                .ThenInclude(bp => bp.Category)
                .OrderByDescending(bp => bp.Product.CreatedAt)
                .FiltersBranchProduct(name)
                .ToPaginatedListAsync(page, pageSize);

        public async Task<IEnumerable<BranchProduct>> GetBranchProductsByProductIdsAsync(Guid branchId, IEnumerable<int> productIds) =>
             await context.BranchProducts
                .Include(bp => bp.Product)
                .Where(bp => bp.BranchId == branchId && productIds.Contains(bp.ProductId))
                .ToListAsync();

        public async Task CreateSaleAsync(Sale sale, List<InventoryMovement> intentoryMovements, List<BranchProduct> productsUpdated, AuditHistory auditHistory)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();

            sale.Folio = $"POS-{await GetNextFolioNumberAsync(sale.BusinessId):D4}";

            context.Sales.Add(sale);
            context.InventoryMovements.AddRange(intentoryMovements);
            context.BranchProducts.UpdateRange(productsUpdated);
            context.AuditHistories.Add(auditHistory);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();
        }

        private async Task<int> GetNextFolioNumberAsync(Guid businessId)
        {
            var connection = context.Database.GetDbConnection();
            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.Transaction = context.Database.CurrentTransaction?.GetDbTransaction();
            command.CommandText = @"
                INSERT INTO ""BusinessFolioCounters"" (""BusinessId"", ""LastFolioNumber"")
                VALUES (@bid, 1)
                ON CONFLICT (""BusinessId"") DO UPDATE
                SET ""LastFolioNumber"" = ""BusinessFolioCounters"".""LastFolioNumber"" + 1
                RETURNING ""LastFolioNumber""";
            var param = command.CreateParameter();
            param.ParameterName = "@bid";
            param.Value = businessId;
            command.Parameters.Add(param);

            return Convert.ToInt32(await command.ExecuteScalarAsync());
        }

        public async Task<PaginatedList<Sale>> GetSalesByBranchAsync(Guid businessId, Guid id, DateTime? fromDate, DateTime? toDate, int page, int pageSize) =>
            await context.Sales
                .AsQueryable()
                .Where(s => s.BusinessId == businessId)
                .Include(s => s.Branch)
                .Include(s => s.Seller)
                .Include(s => s.Customer)
                .Include(s => s.SaleDetails)
                    .ThenInclude(sd => sd.Product)
                .Where(s => s.BranchId == id)
                .FiltersSales(fromDate, toDate)
                .OrderByDescending(b => b.Date)
                .ToPaginatedListAsync(page, pageSize);

        public async Task<BranchProduct?> GetBranchProductByBranchIdAndProductIdAsync(Guid? branchId, int productId) =>
             await context.BranchProducts
                .Include(bp => bp.Product)
                .FirstOrDefaultAsync(bp => branchId.HasValue && bp.BranchId == branchId && bp.ProductId == productId);

        public async Task AddProductsToBranchAsync(IEnumerable<BranchProduct> branchProducts)
        {
            context.BranchProducts.AddRange(branchProducts);
            await context.SaveChangesAsync();
        }

        public async Task<PaginatedList<Product>> GetProductsDoesntExistByBranchAsync(Guid id, int page, int pageSize) =>
            await context.Products
                .AsQueryable()
                .Where(p => !context.BranchProducts.Any(bp => bp.BranchId == id && bp.ProductId == p.Id))
                .Include(p => p.Measure)
                .Include(p => p.Category)
                .ToPaginatedListAsync(page, pageSize);
    }
}
