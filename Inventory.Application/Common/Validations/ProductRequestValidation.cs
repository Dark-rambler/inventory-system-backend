using FluentValidation;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.DataTransferObjects.ProductDto;

namespace Inventory.Application.Common.Validations
{
    public class ProductRequestValidation : AbstractValidator<ProductRequest>
    {
        public ProductRequestValidation(ICategoryRepository repository)
        {
        }
    }
}
