using FluentValidation;
using Inventory.Application.DataTransferObjects.CustomerDto;

namespace Inventory.Application.Common.Validations
{
    public class CustomerRequestValidation : AbstractValidator<CustomerRequest>
    {
        public CustomerRequestValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(100);
            RuleFor(x => x.Nit)
                .NotEmpty()
                .WithMessage("Nit is required")
                .MaximumLength(20);
            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Phone is required")
                .MaximumLength(20);
        }
    }
}