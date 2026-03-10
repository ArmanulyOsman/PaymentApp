using Microsoft.EntityFrameworkCore;
using PaymentApp.Models;

namespace PaymentApp.DB;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.WalletNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Account).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(254);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Currency).HasConversion<string>();
            entity.Property(e => e.Comment).HasMaxLength(500);
            entity.Property(e => e.Status).HasConversion<string>();
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.Account);
        });
    }
}
