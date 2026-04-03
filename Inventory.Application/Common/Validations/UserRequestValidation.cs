using FluentValidation;
using Inventory.Application.DataTransferObjects.UserDto;

namespace Inventory.Application.Common.Validations
{
    public class UserRequestValidation : AbstractValidator<UserRequest>
    {
        public UserRequestValidation()
        {
        }
    }
}
