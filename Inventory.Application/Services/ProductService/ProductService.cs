using AutoMapper;
using FluentValidation;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Abstracts.Clients;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Services.ProductService
{
    public class ProductService(IProductRepository repository, IExcelReader excelReader, IMapper mapper, IValidator<ProductRequest> validator) : IProductService
    {
        public async Task<PaginatedList<ProductResponse>> GetProductsAsync(ProductSearchParams searchParams, Guid businessId)
        {
            var product = await repository.GetProductsAsync(businessId, searchParams.Name, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<ProductResponse>(
                mapper.Map<List<ProductResponse>>(product.Items),
                product.TotalCount,
                product.PageIndex,
                product.PageSize
            );
        }

        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            return mapper.Map<ProductResponse>(await FindProductById(id));
        }

        public async Task<ProductResponse> CreateProductAsync(ProductRequest request, Guid businessId)
        {
            await validator.ValidateAndThrowAsync(request);
            var product = mapper.Map<Product>(request);
            product.BusinessId = businessId;
            return mapper.Map<ProductResponse>(await repository.CreateProductAsync(product));
        }

        public async Task UpdateProductAsync(int id, ProductRequest request)
        {
            await validator.ValidateAndThrowAsync(request);
            await repository.UpdateProductAsync(mapper.Map(request, await FindProductById(id)));
        }

        private async Task<Product> FindProductById(int id)
        {
            return await repository.GetProductByIdAsync(id) ?? throw new KeyNotFoundException($"Product with id {id} doesn't exist");
        }

        public async Task DeleteProductAsync(int id)
        {
            await repository.DeleteProductAsync(await FindProductById(id));
        }

        public async Task BulkUploadProductsAsync(Stream fileStream)
        {
            var products = await excelReader.ReadProductsAsync(fileStream);
            await repository.BulkCreateAsync(products);
        }

        public Stream GetBulkUploadTemplate()
        {
            return excelReader.GenerateProductsTemplate();
        }
    }
}
