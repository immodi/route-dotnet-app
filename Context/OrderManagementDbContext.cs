
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


public class OrderManagementDbContext : IdentityDbContext<AuthUser>{
    public DbSet<Customer> TCustomers { get; set; }
    public DbSet<Order> TOrders { get; set; }
    public DbSet<OrderItem> TOrderItems { get; set; }
    public DbSet<Product> TProducts { get; set; }
    public DbSet<Invoice> TInvoices { get; set; }
    public DbSet<User> TUsers { get; set; }

    // private readonly RoleManager<IdentityRole> _roleManager;
    public OrderManagementDbContext (DbContextOptions<OrderManagementDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId);

        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Invoice)
            .WithOne(i => i.Order)
            .HasForeignKey<Invoice>(i => i.OrderId);

        modelBuilder.Entity<Product>()
            .HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Product)
            .HasForeignKey(oi => oi.ProductId);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        // modelBuilder.Entity<AuthIser>()
        //     .HasIndex(u => u.UserName)
        //     .IsUnique();

        base.OnModelCreating(modelBuilder);

    }
}
