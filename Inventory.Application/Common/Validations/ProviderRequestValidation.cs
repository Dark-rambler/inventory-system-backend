using FluentValidation;
using Inventory.Application.DataTransferObjects.ProviderDto;

namespace Inventory.Application.Common.Validations
{
    public class ProviderRequestValidation : AbstractValidator<ProviderRequest>
    {
        public ProviderRequestValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es requerido")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

            RuleFor(x => x.Contact)
                .NotEmpty().WithMessage("El contacto es requerido")
                .MaximumLength(100).WithMessage("El contacto no puede exceder 100 caracteres");

            RuleFor(x => x.Telephone)
                .NotEmpty().WithMessage("El teléfono es requerido")
                .MaximumLength(20).WithMessage("El teléfono no puede exceder 20 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es requerido")
                .EmailAddress().WithMessage("El email debe ser válido")
                .MaximumLength(100).WithMessage("El email no puede exceder 100 caracteres");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("La ciudad es requerida")
                .MaximumLength(50).WithMessage("La ciudad no puede exceder 50 caracteres");
        }
    }
}