using Civilians.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Civilians.Infrastructure.DbContext.Configuration
{
    internal static class UsersConfiguration
    {
        internal static void ConfigureUsersTable(this ModelBuilder builder)
        {
            builder.Entity<User>(opt =>
            {
                opt.Ignore(u => u.PhoneNumberConfirmed);
                opt.Ignore(c => c.EmailConfirmed);
                opt.Ignore(c => c.TwoFactorEnabled);
                opt.Ignore(c => c.LockoutEnd);

                opt.Property("CreatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");

                opt.Property("UpdatedAt")
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnUpdate();
            });
        }
    }
}
