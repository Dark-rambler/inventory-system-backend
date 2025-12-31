using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Context
{
    public class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<Category>()
                .Property(c => c.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            base.OnModelCreating(modelBuilder);
        }
    }
}
