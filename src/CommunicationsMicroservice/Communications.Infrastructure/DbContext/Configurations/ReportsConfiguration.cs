using Communications.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Communications.Infrastructure.DbContext.Configurations;

public class ReportsConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.Ignore(r => r.CanBeEdited);

        builder
            .HasOne(report => report.Reply)
            .WithOne(reply => reply.Report)
            .HasForeignKey<Reply>(reply => reply.ReportId);

        builder.Property(r => r.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd();

        builder.Property(r => r.UpdatedAt)
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAddOrUpdate();
    }
}