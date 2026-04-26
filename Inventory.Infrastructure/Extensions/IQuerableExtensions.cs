using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;
using Inventory.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Extensions
{
    public static class IQuerableExtensions
    {
        extension<T>(IQueryable<T> source)
        {
            public async Task<PaginatedList<T>> ToPaginatedListAsync(int pageIndex, int pageSize)
            {
                var count = await source.CountAsync();
                var items = await source
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PaginatedList<T>(items, count, pageIndex, pageSize);
            }
        }

        extension(IQueryable<Product> source)
        {
            public IQueryable<Product> FiltersProduct(string? name)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    source = source.Where(
                        c => c.Name.ToLower().Contains(name.ToLower())
                        || c.Category!.Name.ToLower().Contains(name.ToLower())
                        || c.Code.ToLower().Contains(name.ToLower())
                    );
                }
                return source;
            }
        }

        extension(IQueryable<Category> source)
        {
            public IQueryable<Category> FiltersCategory(string? name)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    source = source.Where(c => c.Name.ToLower().Contains(name.ToLower()));
                }
                return source;
            }
        }

        extension(IQueryable<Branch> source)
        {
            public IQueryable<Branch> FiltersBranch(string? name)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    source = source.Where(c => c.Name.ToLower().Contains(name.ToLower()));
                }
                return source;
            }
        }

        extension(IQueryable<Warehouse> source)
        {
            public IQueryable<Warehouse> FiltersWarehouse(string? name, Guid? productId = null, int? stock = null)
            {
                if (productId.HasValue)
                {
                    source = source.Where(c => c.WarehouseProducts.Any(wp => wp.ProductId == productId));
                }
                if (productId.HasValue)
                {
                    source = source.Where(c => c.WarehouseProducts.Any(wp => wp.Stock >= stock));
                }
                if (!string.IsNullOrEmpty(name))
                {
                    source = source.Where(c => c.Name.ToLower().Contains(name.ToLower()));
                }
                return source;
            }
        }

        extension(IQueryable<User> source)
        {
            public IQueryable<User> FiltersUser(string? name)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    source = source.Where(c => c.Name.ToLower().Contains(name.ToLower()));
                }
                return source;
            }
        }

        extension(IQueryable<BranchProduct> source)
        {
            public IQueryable<BranchProduct> FiltersBranchProduct(string? name)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    source = source.Where(c => c.Product.Name.ToLower().Contains(name.ToLower()));
                }
                return source;
            }
        }

        extension(IQueryable<WarehouseProduct> source)
        {
            public IQueryable<WarehouseProduct> FiltersWarehouseProduct(string? name)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    source = source.Where(c => c.Product.Name.ToLower().Contains(name.ToLower()));
                }
                return source;
            }
        }

        extension(IQueryable<Sale> source)
        {
            public IQueryable<Sale> FiltersSales( DateTime? fromDate, DateTime? toDate)
            {
                if(fromDate.HasValue)
                {
                    source = source.Where(s => s.Date >= fromDate.Value);
                }
                if(toDate.HasValue)
                {
                    source = source.Where(s => s.Date <= toDate.Value);
                }
                return source;
            }
        }

        extension(IQueryable<Purchase> source)
        {
            public IQueryable<Purchase> FiltersPurchases(DateTime? fromDate, DateTime? toDate, Guid? providerId, Guid? branchId)
            {
                if(fromDate.HasValue)
                {
                    source = source.Where(s => s.Date >= fromDate.Value);
                }
                if(toDate.HasValue)
                {
                    source = source.Where(s => s.Date <= toDate.Value);
                }
                if(providerId.HasValue)
                {
                    source = source.Where(s => s.ProviderId == providerId.Value);
                }
                if(branchId.HasValue)
                {
                    source = source.Where(s => s.BranchId == branchId.Value);
                }
                return source;
            }
        }

        extension(IQueryable<Customer> source)
        {
            public IQueryable<Customer> FiltersCustomer(string? name)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    source = source.Where(c => c.Name.ToLower().Contains(name.ToLower()));
                }
                return source;
            }
        }

        extension(IQueryable<Provider> source)
        {
            public IQueryable<Provider> FiltersProvider(string? name)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    source = source.Where(c => c.Name.ToLower().Contains(name.ToLower()));
                }
                return source;
            }
        }

        extension(IQueryable<AuditHistory> source)
        {
            public IQueryable<AuditHistory> FiltersAuditHistory(Guid? userId, EnumAction? action, EnumEntity? entity, DateTime? fromDate, DateTime? toDate)
            {
                if (userId.HasValue)
                {
                    source = source.Where(s => s.UserId == userId);
                }
                if (action.HasValue)
                {
                    source = source.Where(s => s.Action == action.Value);
                }
                if (entity.HasValue)
                {
                    source = source.Where(s => s.Entity == entity.Value);
                }
                if (fromDate.HasValue)
                {
                    source = source.Where(s => s.CreatedAt >= fromDate.Value);
                }
                if (toDate.HasValue)
                {
                    source = source.Where(s => s.CreatedAt <= toDate.Value);
                }
                return source;
            }
        }
    }
}
