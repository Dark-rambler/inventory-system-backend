namespace Inventory.Domain.Entities.Builders
{
    public class RefreshTokenBuilder
    {
        private readonly RefreshToken _refreshToken = new();

        public RefreshTokenBuilder WithToken(string token)
        {
            _refreshToken.Token = token;
            return this;
        }

        public RefreshTokenBuilder WithUserId(Guid userId)
        {
            _refreshToken.UserId = userId;
            return this;
        }

        public RefreshTokenBuilder WithExpiresAt(DateTime expiresAt)
        {
            _refreshToken.ExpiresAt = expiresAt;
            return this;
        }

        public RefreshTokenBuilder WithCreatedAt(DateTime createdAt)
        {
            _refreshToken.CreatedAt = createdAt;
            return this;
        }

        public RefreshToken Build() => _refreshToken;
    }
}
