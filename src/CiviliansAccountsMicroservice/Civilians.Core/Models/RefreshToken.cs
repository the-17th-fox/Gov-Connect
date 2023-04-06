using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Civilians.Core.Models
{
    [Index(nameof(UserId), IsUnique = true)]
    public class RefreshToken
    {
        [Key]
        public Guid Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
        public bool IsRevoked { get; set; } = false;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        [NotMapped]
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        [NotMapped]
        public bool IsActive => !(IsExpired || IsRevoked);

        public RefreshToken(DateTime expiresAt, Guid userId)
        {
            ExpiresAt = expiresAt;
            UserId = userId;
        }
    }
}
