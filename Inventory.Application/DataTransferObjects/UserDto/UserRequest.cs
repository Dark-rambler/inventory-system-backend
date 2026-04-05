namespace Inventory.Application.DataTransferObjects.UserDto
{
    public class UserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
