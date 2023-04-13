namespace Civilians.Core.Models
{
    public class RefreshToken
    {
        public Guid Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
        public bool IsRevoked { get; set; } = false;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => !(IsExpired || IsRevoked);

        public RefreshToken(DateTime expiresAt, Guid userId)
        {
            ExpiresAt = expiresAt;
            UserId = userId;
        }
    }
}
