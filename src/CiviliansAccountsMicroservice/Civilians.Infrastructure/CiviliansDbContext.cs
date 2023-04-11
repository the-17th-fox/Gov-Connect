using Civilians.Core.Auth;
using Civilians.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Civilians.Infrastructure
{
    public class CiviliansDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public CiviliansDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Passport> Passports => Set<Passport>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole<Guid>>().HasData(AuthRoles.Roles);

            ConfigureUsers(builder);
            ConfigurePassports(builder);
            ConfigureRefreshTokens(builder);            
        }

        private void ConfigureUsers(ModelBuilder builder)
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

        private void ConfigurePassports(ModelBuilder builder)
        {
            builder.Entity<RefreshToken>(opt =>
            {
                opt.Property("CreatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");

                opt.Property("UpdatedAt")
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnUpdate();
            });
        }

        private void ConfigureRefreshTokens(ModelBuilder builder)
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
            });
        }
    }
}
