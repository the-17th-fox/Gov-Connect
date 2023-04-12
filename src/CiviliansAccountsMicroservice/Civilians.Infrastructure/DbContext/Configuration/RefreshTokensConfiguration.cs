using Civilians.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Civilians.Infrastructure.DbContext.Configuration
{
    internal static class RefreshTokensConfiguration
    {
        internal static void ConfigureRefreshTokensTable(this ModelBuilder builder)
        {
            builder.Entity<RefreshToken>(opt =>
            {
                opt.Ignore(rt => rt.IsActive);
                opt.Ignore(rt => rt.IsExpired);

                opt.Property("CreatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");

                opt.Property("UpdatedAt")
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnUpdate();

                opt.HasIndex(rt => rt.UserId)
                    .IsUnique();
            });
        }
    }
}
