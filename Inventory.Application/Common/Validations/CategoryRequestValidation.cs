using FluentValidation;
using Inventory.Application.DataTransferObjects.CategoryDto;

namespace Inventory.Application.Common.Validations
{
    public class CategoryRequestValidation : AbstractValidator<CategoryRequest>
    {
        public CategoryRequestValidation()
        {
        }
    }
}
