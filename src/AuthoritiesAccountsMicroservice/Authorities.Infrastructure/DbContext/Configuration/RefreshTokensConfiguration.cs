using Authorities.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorities.Infrastructure.DbContext.Configuration
{
    internal class RefreshTokensConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.Ignore(rt => rt.IsActive);
            builder.Ignore(rt => rt.IsExpired);

            builder.HasKey(rt => rt.Token);

            builder.Property<DateTime>("CreatedAt")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property<DateTime>("UpdatedAt")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();

            builder.HasIndex(rt => rt.UserId)
                .IsUnique();
        }
    }
}
