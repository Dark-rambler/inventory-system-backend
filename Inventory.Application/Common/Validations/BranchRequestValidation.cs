using FluentValidation;
using Inventory.Application.DataTransferObjects.BranchDto;

namespace Inventory.Application.Common.Validations
{
    public class BranchRequestValidation : AbstractValidator<BranchRequest>
    {
        public BranchRequestValidation()
        {
            RuleFor(b => b.Name)
                .NotEmpty().WithMessage("Branch name is required.")
                .MaximumLength(100).WithMessage("Branch name must not exceed 100 characters.");
            RuleFor(b => b.Address)
                .NotEmpty().WithMessage("Branch address is required.")
                .MaximumLength(200).WithMessage("Branch location must not exceed 200 characters.");
            RuleFor(b => b.Telephone)
                .Length(10).WithMessage("Branch telephone must be exactly 12 characters.");
        }
    }
}
