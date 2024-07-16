using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SqlServerWebApi.Models
{


public class OrderManagementDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<User> Users { get; set; }
    private readonly RoleManager<IdentityRole> _roleManager;

    public OrderManagementDbContext(DbContextOptions<OrderManagementDbContext> options, RoleManager<IdentityRole> roleManager):base(options){

        _roleManager = roleManager;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
            
    // public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the relationships
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
    }

     private async Task CreateRolesandUsers()
    {  
        bool x = await _roleManager.RoleExistsAsync("Admin");
        if (!x)
        {
            // first we create Admin rool    
            var role = new IdentityRole();
            role.Name = "Admin";
            await _roleManager.CreateAsync(role);

            //Here we create a Admin super user who will maintain the website                   

            var user = new User();
            user.UserName = "admin";
            user.Email = "admin@admin.com";

            string userPWD = "somepassword";

            IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);

            //Add default User to Role Admin    
            if (chkUser.Succeeded)
            {
                var result1 = await _userManager.AddToRoleAsync(user, "Admin");
            }
        }

        // creating Creating Manager role     
        x = await _roleManager.RoleExistsAsync("Manager");
        if (!x)
        {
            var role = new IdentityRole();
            role.Name = "Manager";
            await _roleManager.CreateAsync(role);
        }

        // creating Creating Employee role     
        x = await _roleManager.RoleExistsAsync("Employee");
        if (!x)
        {
            var role = new IdentityRole();
            role.Name = "Employee";
            await _roleManager.CreateAsync(role);
        }
  }
}
}
