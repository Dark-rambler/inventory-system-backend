using FluentValidation;
using Inventory.Application.DataTransferObjects.PurchaseDto;

namespace Inventory.Application.Common.Validations
{
    public class PurchaseRequestValidation : AbstractValidator<PurchaseRequest>
    {
        public PurchaseRequestValidation()
        {
            RuleFor(x => x.ProviderId)
                .NotEmpty()
                .WithMessage("Provider is required.");
            RuleFor(x => x.BranchId)
                .NotEmpty()
                .WithMessage("Branch is required.");
            RuleFor(x => x.PurchaseDetails)
                .NotEmpty()
                .WithMessage("Purchase details are required.");
            RuleForEach(x => x.PurchaseDetails)
                .ChildRules(pd =>
                {
                    pd.RuleFor(y => y.ProductId)
                        .NotEmpty()
                        .WithMessage("Product is required.");
                    pd.RuleFor(y => y.Quantity)
                        .GreaterThan(0)
                        .WithMessage("Quantity must be greater than 0.");
                });
        }
    }
}