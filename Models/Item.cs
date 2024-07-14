using Microsoft.EntityFrameworkCore;

namespace SqlServerWebApi.Models
{

    public class ItemsContext : DbContext
    {
        private readonly string _connectionString;

        public ItemsContext(string connectionString) {
            _connectionString = connectionString;
        }

        public DbSet<Item> Inventory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Quantity { get; set; }
    }
}
