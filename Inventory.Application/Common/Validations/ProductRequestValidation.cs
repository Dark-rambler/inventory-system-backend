using FluentValidation;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.DataTransferObjects.ProductDto;

namespace Inventory.Application.Common.Validations
{
    public class ProductRequestValidation : AbstractValidator<ProductRequest>
    {
        public ProductRequestValidation(ICategoryRepository categoryRepository)
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");
            
            RuleFor(p => p.Code)
                .NotEmpty().WithMessage("Product code is required.")
                .MaximumLength(50).WithMessage("Product code must not exceed 50 characters.");
            
            RuleFor(p => p.Description)
                .MaximumLength(1000).WithMessage("Product description must not exceed 1000 characters.");
            
            RuleFor(p => p.CategoryId)
                .NotEmpty().WithMessage("Category ID is required.")
                .MustAsync(async (id, cancellation) =>
                {
                    var category = await categoryRepository.GetCategoryByIdAsync(id);
                    return category != null;
                })
                .WithMessage("The specified Category ID does not exist.");
        }
    }
}
