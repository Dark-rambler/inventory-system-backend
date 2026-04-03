using FluentValidation;
using Inventory.Application.DataTransferObjects.BranchDto;

namespace Inventory.Application.Common.Validations
{
    public class BranchRequestValidation : AbstractValidator<BranchRequest>
    {
        public BranchRequestValidation()
        {
        }
    }
}
