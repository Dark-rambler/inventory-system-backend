using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Services.ProductService
{
    public class ProductService(IProductRepository repository, IMapper mapper) : IProductService
    {
        public async Task<PaginatedList<ProductResponse>> GetProductsAsync(string? name, int page, int pageSize)
        {
            var product = await repository.GetProductsAsync(name, page, pageSize);
            return new PaginatedList<ProductResponse>(
                mapper.Map<List<ProductResponse>>(product.Items),
                product.TotalCount,
                product.PageIndex,
                product.PageSize
            );
        }

        public async Task<ProductResponse> GetProductByIdAsync(Guid id)
        {
            return mapper.Map<ProductResponse>(await FindProductById(id));
        }

        public async Task<ProductResponse> CreateProductAsync(ProductRequest request)
        {
            var product = mapper.Map<Product>(request);
            return mapper.Map<ProductResponse>(await repository.CreateProductAsync(product));
        }

        public async Task UpdateProductAsync(Guid id, ProductRequest request)
        {
            var product = await FindProductById(id);
            await repository.UpdateProductAsync(mapper.Map(request, product));
        }

        private async Task<Product> FindProductById(Guid id)
        {
            return await repository.GetProductByIdAsync(id) ?? throw new KeyNotFoundException($"Product with id {id} doesn't exist");
        }
    }
}
