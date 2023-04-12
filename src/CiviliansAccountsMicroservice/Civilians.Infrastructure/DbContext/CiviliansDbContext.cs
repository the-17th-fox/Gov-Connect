using Civilians.Core.Auth;
using Civilians.Core.Models;
using Civilians.Infrastructure.DbContext.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Civilians.Infrastructure.DbContext
{
    public class CiviliansDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public CiviliansDbContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
            Database.EnsureCreated();
        }

        public DbSet<Passport> Passports => Set<Passport>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole<Guid>>().HasData(AuthRoles.Roles);

            builder.ConfigureUsersTable();
            builder.ConfigurePassportsTable();
            builder.ConfigureRefreshTokensTable();
        }
    }
}
