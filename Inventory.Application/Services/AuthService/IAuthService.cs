using Inventory.Application.DataTransferObjects.AuthDto;

namespace Inventory.Application.Services.AuthService
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
