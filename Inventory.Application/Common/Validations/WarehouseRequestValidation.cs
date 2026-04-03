using FluentValidation;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.DataTransferObjects.WarehouseDto;

namespace Inventory.Application.Common.Validations
{
    public class WarehouseRequestValidation : AbstractValidator<WarehouseRequest>
    {
        public WarehouseRequestValidation(IBranchRepository repository)
        {
        }
    }
}
