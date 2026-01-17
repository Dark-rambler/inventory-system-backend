using FluentValidation;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.DataTransferObjects.WarehouseDto;

namespace Inventory.Application.Common.Validations
{
    public class WarehouseRequestValidation : AbstractValidator<WarehouseRequest>
    {
        public WarehouseRequestValidation(IWarehouseRepository repository)
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Warehouse name is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");
            RuleFor(p => p.BranchId)
                .NotEmpty().WithMessage("Branch is required.")
                .MustAsync(async (id, cancellation) =>
                {
                    var category = await repository.GetWarehouseByIdAsync(id);
                    return category != null;
                })
                .WithMessage("The specified Branch does not exist.");
        }
    }
}
