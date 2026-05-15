using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Abstracts.Clients;
using Inventory.Application.DataTransferObjects.AuthDto;

namespace Inventory.Application.Services.AuthService
{
    public class AuthService(
        IUserRepository repository,
        IJwtService jwtService,
        IPasswordHasher passwordHasher
    ) : IAuthService
    {
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await repository.GetUserByUserNameAsync(request.UserName)
                ?? throw new UnauthorizedAccessException("Invalid credentials");

            if (!passwordHasher.Verify(request.Password, user.Password))
                throw new UnauthorizedAccessException("Invalid credentials");

            var token = jwtService.GenerateJwtToken(user);
            return new LoginResponse(token);
        }
    }
}
