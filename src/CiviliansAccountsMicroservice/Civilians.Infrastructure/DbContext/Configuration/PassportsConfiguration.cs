using Civilians.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Civilians.Infrastructure.DbContext.Configuration
{
    internal class PassportsConfiguration : IEntityTypeConfiguration<Passport>
    {
        public void Configure(EntityTypeBuilder<Passport> builder)
        {
            builder.Property<DateTime>("CreatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");

            builder.Property<DateTime>("UpdatedAt")
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnUpdate();

            builder.HasIndex(p => new { p.FirstName, p.LastName, p.Patronymic });
        }
    }
}
