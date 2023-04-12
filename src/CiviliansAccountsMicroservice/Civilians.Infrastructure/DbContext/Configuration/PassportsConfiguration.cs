using Civilians.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Civilians.Infrastructure.DbContext.Configuration
{
    internal static class PassportsConfiguration
    {
        internal static void ConfigurePassportsTable(this ModelBuilder builder)
        {
            builder.Entity<Passport>(opt =>
            {
                opt.Property("CreatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");

                opt.Property("UpdatedAt")
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnUpdate();

                opt.HasIndex(p => new { p.FirstName, p.LastName, p.Patronymic });
            });
        }
    }
}
