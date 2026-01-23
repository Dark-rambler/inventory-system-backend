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
        public async Task<PaginatedList<CategoryResponse>> GetCategoriesAsync(CategorySearchParams searchParams)
        {
            var categories = await repository.GetCategoriesAsync(searchParams.Name, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<CategoryResponse>(
                mapper.Map<List<CategoryResponse>>(categories.Items),
                categories.TotalCount,
                categories.PageIndex,
                categories.PageSize
            );
        }

        public async Task<CategoryResponse> GetCategoryByIdAsync(Guid id)
        {
            return mapper.Map<CategoryResponse>(await FindCategoryById(id));
        }

        public async Task<CategoryResponse> CreateCategoryAsync(CategoryRequest request)
        {
            await validator.ValidateAndThrowAsync(request);
            return mapper.Map<CategoryResponse>(await repository.CreateCategoryAsync(mapper.Map<Category>(request)));
        }

        public async Task UpdateCategoryAsync(Guid id, CategoryRequest request)
        {
            await validator.ValidateAndThrowAsync(request);
            await repository.UpdateCategoryAsync(mapper.Map(request, await FindCategoryById(id)));
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            await repository.DeleteCategoryAsync(await FindCategoryById(id));
        }

        private async Task<Category> FindCategoryById(Guid id)
        {
            return await repository.GetCategoryByIdAsync(id) ?? throw new KeyNotFoundException($"Category with id {id} doesn't exist");
        }
    }
}
