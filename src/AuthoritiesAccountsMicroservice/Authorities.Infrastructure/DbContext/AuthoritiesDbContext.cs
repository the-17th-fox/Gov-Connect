using Authorities.Core.Auth;
using Authorities.Core.Models;
using Authorities.Infrastructure.DbContext.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authorities.Infrastructure.DbContext
{
    public class AuthoritiesDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AuthoritiesDbContext(DbContextOptions options) : base(options) {}

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole<Guid>>().HasData(AuthRoles.Roles);

            builder.ApplyConfiguration(new UsersConfiguration());
            builder.ApplyConfiguration(new RefreshTokensConfiguration());
        }
    }
}
