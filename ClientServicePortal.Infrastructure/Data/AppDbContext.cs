using ClientServicePortal.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClientServicePortal.Infrastructure.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<User>(e =>
      {
        e.Property(p => p.FullName).HasMaxLength(100);
        e.Property(p => p.Email).IsRequired().HasMaxLength(100);
        e.HasIndex(p => p.Email).IsUnique();
        e.Property(p => p.Role).HasMaxLength(20);
      });

      modelBuilder.Entity<ServiceRequest>()
        .HasOne(r => r.User)
        .WithMany(u => u.Requests)
        .HasForeignKey(r => r.UserId);

      modelBuilder.Entity<Payment>()
        .HasOne(p => p.User)
        .WithMany(u => u.Payments)
        .HasForeignKey(p => p.UserId);

      modelBuilder.Entity<Payment>()
        .HasOne(p => p.Request)
        .WithMany(r => r.Payments)
        .HasForeignKey(p => p.RequestId);
    }
  }
}
