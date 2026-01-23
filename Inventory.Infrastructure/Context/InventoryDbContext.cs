using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Context
{
    public class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<Category>()
                .Property(c => c.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<Branch>()
                .Property(c => c.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<Warehouse>()
                .Property(c => c.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Product>()
                .HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Category>()
                .HasQueryFilter(c => !c.IsDeleted);
            base.OnModelCreating(modelBuilder);
        }
    }
}
