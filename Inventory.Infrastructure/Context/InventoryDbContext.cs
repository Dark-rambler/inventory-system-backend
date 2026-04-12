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
        public DbSet<User> Users { get; set; }
        public DbSet<BranchProduct> BranchProducts { get; set; }
        public DbSet<WarehouseProduct> WarehouseProducts { get; set; }
        public DbSet<InventoryMovement> InventoryMovements { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Measure> Measures { get; set; }
        public DbSet<Role> Roles { get; set; }
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
            modelBuilder.Entity<User>()
                .Property(c => c.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<InventoryMovement>()
                .Property(c => c.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<Sale>()
                .Property(c => c.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<BranchProduct>()
                .HasKey(bp => new { bp.BranchId, bp.ProductId });
            modelBuilder.Entity<WarehouseProduct>()
                .HasKey(wp => new { wp.WarehouseId, wp.ProductId });
            modelBuilder.Entity<Product>()
                .HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Category>()
                .HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Warehouse>()
                .HasQueryFilter(w => !w.IsDeleted);
            modelBuilder.Entity<Branch>()
                .HasQueryFilter(b => !b.IsDeleted);
            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<Location>()
                .HasQueryFilter(l => !l.IsDeleted);
            base.OnModelCreating(modelBuilder);
        }
    }
}
