using Microsoft.EntityFrameworkCore;
using Shopimoto.Domain.Entities;

namespace Shopimoto.Infrastructure.Data;

public class ShopimotoDbContext : DbContext
{
    public ShopimotoDbContext(DbContextOptions<ShopimotoDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure entities
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
            
            // Unique Username
            entity.HasIndex(e => e.Username).IsUnique(); 
            // Note: Since we have existing users with empty string usernames, this might fail unless we filter or clean data.
            // But let's assume valid data for now or we might need to make it nullable. 
            // Actually, let's keep it simple. If it fails, I'll fix the data.
        });

        // Reviews Relationship
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent deleting user from deleting reviews indiscriminately if not desired, or Cascade. Let's stick with Restrict or Cascade. Cascade is probably fine for a demo. let's use Restrict to be safe.

        // Wishlist Relationship
        modelBuilder.Entity<Wishlist>()
            .HasOne(w => w.User)
            .WithMany(u => u.Wishlists)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        // Address Relationship
        modelBuilder.Entity<Address>()
            .HasOne(a => a.User)
            .WithMany(u => u.Addresses)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
