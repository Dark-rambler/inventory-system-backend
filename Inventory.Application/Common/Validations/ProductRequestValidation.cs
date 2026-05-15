using FluentValidation;
using Inventory.Application.DataTransferObjects.ProductDto;

namespace Inventory.Application.Common.Validations
{
    public class ProductRequestValidation : AbstractValidator<ProductRequest>
    {
        public ProductRequestValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.")
                .MaximumLength(50).WithMessage("Code cannot exceed 50 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("A valid category is required.");
        }
    }
}
