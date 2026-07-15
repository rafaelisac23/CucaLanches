using CucaLanches.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CucaLanches.Infrastructure;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
    }
    
    public DbSet<Product> Products=> Set<Product>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Neighborhood> Neighborhoods=> Set<Neighborhood>();
    public DbSet<StoreSettings> StoreSettings => Set<StoreSettings>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Product>(e =>
        {
        e.Property(p=> p.Name).HasMaxLength(120);
        e.Property(p => p.Type).HasConversion<string>().HasMaxLength(20);
        e.Property(p => p.Price).HasPrecision(10, 2);
        });

        mb.Entity<User>(e =>
        {
            e.HasIndex(p => p.Email).IsUnique();
            e.Property(p => p.Role).HasConversion<string>().HasMaxLength(20);
        });

        mb.Entity<Neighborhood>(p =>
        {
            p.HasIndex(e => e.Name).IsUnique();
            p.Property(e=> e.DeliveryFee).HasPrecision(10, 2);
        });
    }
    
}