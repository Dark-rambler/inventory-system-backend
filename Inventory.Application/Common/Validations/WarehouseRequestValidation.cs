using FluentValidation;
using Inventory.Application.DataTransferObjects.WarehouseDto;

namespace Inventory.Application.Common.Validations
{
    public class WarehouseRequestValidation : AbstractValidator<WarehouseRequest>
    {
        public WarehouseRequestValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Location)
                .NotNull().WithMessage("Location is required.");
        }
    }
}
