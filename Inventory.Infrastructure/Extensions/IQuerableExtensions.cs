using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;
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
    }
}
