using Civilians.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Civilians.Infrastructure.DbContext.Configuration
{
    internal class RefreshTokensConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.Ignore(rt => rt.IsActive);
            builder.Ignore(rt => rt.IsExpired);

            builder.HasKey(rt => rt.Token);

            builder.Property<DateTime>("CreatedAt")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property<DateTime>("UpdatedAt")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnUpdate();

            builder.HasIndex(rt => rt.UserId)
                .IsUnique();
        }
    }
}
