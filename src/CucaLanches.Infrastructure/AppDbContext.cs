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
    public DbSet<StoreSetting> StoreSettings => Set<StoreSetting>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderSequence> OrderSequences => Set<OrderSequence>();

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

        mb.Entity<Client>(c =>
        {
            c.HasIndex(e => e.Phone).IsUnique();
            c.Property(e => e.Phone).HasMaxLength(15);
            c.Property(e => e.Name).HasMaxLength(120);
        });

        mb.Entity<Address>(e =>
        {
            e.HasOne(x => x.Client).WithMany(c => c.Addresses).HasForeignKey(x => x.ClientId);
            e.HasOne(x => x.Neighborhood).WithMany().HasForeignKey(x => x.NeighborhoodId)
                .OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Cep).HasMaxLength(8);
            e.Property(x => x.StreetName).HasMaxLength(150);
        });
        
        mb.Entity<Order>(e =>
        {
            e.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            e.Property(x => x.PaymentMethod).HasConversion<string>().HasMaxLength(10);
            e.Property(x => x.DeliveryFee).HasPrecision(10, 2);
            e.Property(x => x.TotalPrice).HasPrecision(10, 2);
            e.Property(x => x.CashChangeFor).HasPrecision(10, 2);
            e.HasOne(x => x.Address).WithMany().HasForeignKey(x => x.AddressId).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(x => x.CreatedAt);
        });
        
        mb.Entity<OrderItem>(e =>
        {
            e.Property(x => x.UnitPrice).HasPrecision(10, 2);
            e.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        });

        mb.Entity<OrderSequence>(e =>
        {
            e.HasKey(x => x.Date);
        });
    }
    
}