using FluentValidation;
using Inventory.Application.DataTransferObjects.BusinessDto;

namespace Inventory.Application.Common.Validations
{
    public class BusinessRequestValidation : AbstractValidator<BusinessRequest>
    {
        public BusinessRequestValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
        }
    }
}
