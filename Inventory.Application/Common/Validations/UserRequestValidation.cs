using FluentValidation;
using Inventory.Application.DataTransferObjects.UserDto;

namespace Inventory.Application.Common.Validations
{
    public class UserRequestValidation : AbstractValidator<UserRequest>
    {
        public UserRequestValidation()
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("User name is required.")
                .MaximumLength(100).WithMessage("User name must not exceed 100 characters.");
            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters.");
            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(u => u.Role)
                .NotEmpty().WithMessage("Role is required.")
                .MaximumLength(50).WithMessage("Role must not exceed 50 characters.");
        }
    }
}
