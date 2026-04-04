namespace Inventory.Application.DataTransferObjects.AuthDto
{
    public record LoginResponse(string Token, Guid UserId, string UserName, string Role, string Email);
}
