using Civilians.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Civilians.Infrastructure.DbContext.Configuration
{
    internal class UsersConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Ignore(u => u.PhoneNumberConfirmed);
            builder.Ignore(c => c.EmailConfirmed);
            builder.Ignore(c => c.TwoFactorEnabled);
            builder.Ignore(c => c.LockoutEnd);

            builder.Property<DateTime>("CreatedAt")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property<DateTime>("UpdatedAt")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnUpdate();
        }
    }
}
