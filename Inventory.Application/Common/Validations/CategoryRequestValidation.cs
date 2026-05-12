using FluentValidation;
using Inventory.Application.DataTransferObjects.CategoryDto;

namespace Inventory.Application.Common.Validations
{
    public class CategoryRequestValidation : AbstractValidator<CategoryRequest>
    {
        public CategoryRequestValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
