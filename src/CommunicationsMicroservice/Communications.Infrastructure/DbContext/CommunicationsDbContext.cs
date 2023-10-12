using Communications.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Communications.Infrastructure.DbContext;

public class CommunicationsDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public CommunicationsDbContext(DbContextOptions<CommunicationsDbContext> options) : base(options) {}

    public DbSet<Classification> Classifications => Set<Classification>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<Reply> Replies => Set<Reply>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}