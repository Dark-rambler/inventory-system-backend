using FluentValidation;
using Inventory.Application.DataTransferObjects.BranchDto;

namespace Inventory.Application.Common.Validations
{
    public class BranchRequestValidation : AbstractValidator<BranchRequest>
    {
        public BranchRequestValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Telephone)
                .NotEmpty().WithMessage("Telephone is required.")
                .MaximumLength(20).WithMessage("Telephone cannot exceed 20 characters.");

            RuleFor(x => x.Location)
                .NotNull().WithMessage("Location is required.");
        }
    }
}
