namespace Inventory.Application.DataTransferObjects.AuthDto
{
    public record LoginResponse(string Token, string RefreshToken);
}
