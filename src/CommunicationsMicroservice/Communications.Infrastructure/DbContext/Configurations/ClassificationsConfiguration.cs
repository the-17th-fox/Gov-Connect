using Communications.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communications.Infrastructure.DbContext.Configurations;

public class ClassificationsConfiguration : IEntityTypeConfiguration<Classification>
{
    public void Configure(EntityTypeBuilder<Classification> builder)
    {
        builder.Property<DateTime>("CreatedAt")
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd();

        builder.Property<DateTime>("UpdatedAt")
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAddOrUpdate();
    }
}