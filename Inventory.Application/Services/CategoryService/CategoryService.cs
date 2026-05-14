using AutoMapper;
using FluentValidation;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.CategoryDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Services.CategoryService
{
    public class CategoryService(ICategoryRepository repository, IMapper mapper, IValidator<CategoryRequest> validator) : ICategoryService
    {
        public async Task<PaginatedList<CategoryResponse>> GetCategoriesAsync(CategorySearchParams searchParams, Guid businessId)
        {
            var categories = await repository.GetCategoriesAsync(businessId, searchParams.Name, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<CategoryResponse>(
                mapper.Map<List<CategoryResponse>>(categories.Items),
                categories.TotalCount,
                categories.PageIndex,
                categories.PageSize
            );
        }

        public async Task<CategoryResponse> GetCategoryByIdAsync(int id)
        {
            return mapper.Map<CategoryResponse>(await FindCategoryById(id));
        }

        public async Task<CategoryResponse> CreateCategoryAsync(CategoryRequest request, Guid businessId)
        {
            await validator.ValidateAndThrowAsync(request);
            var category = mapper.Map<Category>(request);
            category.BusinessId = businessId;
            return mapper.Map<CategoryResponse>(await repository.CreateCategoryAsync(category));
        }

        public async Task UpdateCategoryAsync(int id, CategoryRequest request)
        {
            await validator.ValidateAndThrowAsync(request);
            await repository.UpdateCategoryAsync(mapper.Map(request, await FindCategoryById(id)));
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await repository.DeleteCategoryAsync(await FindCategoryById(id));
        }

        private async Task<Category> FindCategoryById(int id)
        {
            return await repository.GetCategoryByIdAsync(id) ?? throw new KeyNotFoundException($"Category with id {id} doesn't exist");
        }
    }
}
