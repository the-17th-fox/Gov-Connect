using Authorities.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorities.Infrastructure.DbContext.Configuration
{
    internal class UsersConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Ignore(u => u.PhoneNumberConfirmed);
            builder.Ignore(u => u.EmailConfirmed);
            builder.Ignore(u => u.TwoFactorEnabled);
            builder.Ignore(u => u.LockoutEnd);
            builder.Ignore(u => u.LockoutEnabled);
            builder.Ignore(u => u.AccessFailedCount);

            builder.Property<DateTime>("CreatedAt")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property<DateTime>("UpdatedAt")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
