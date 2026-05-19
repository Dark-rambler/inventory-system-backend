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
        public DbSet<Business> Businesses { get; set; }
        public DbSet<BranchProduct> BranchProducts { get; set; }
        public DbSet<WarehouseProduct> WarehouseProducts { get; set; }
        public DbSet<InventoryMovement> InventoryMovements { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Measure> Measures { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AuditHistory> AuditHistories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<BusinessSaleCounter> BusinessSaleCounters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Business>(entity =>
            {
                entity.ToTable("Businesss");
                entity.Property(b => b.Id).HasDefaultValueSql("uuid_generate_v4()");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Id).UseIdentityByDefaultColumn();
                entity.HasQueryFilter(p => !p.IsDeleted);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(c => c.Id).UseIdentityByDefaultColumn();
                entity.HasQueryFilter(c => !c.IsDeleted);
            });

            modelBuilder.Entity<Branch>(entity =>
            {
                entity.Property(b => b.Id).HasDefaultValueSql("uuid_generate_v4()");
                entity.HasQueryFilter(b => !b.IsDeleted);
                entity.HasOne(b => b.Location)
                    .WithOne(l => l.Branch)
                    .HasForeignKey<Branch>(b => b.Id)
                    .IsRequired();
            });

            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.Property(w => w.Id).HasDefaultValueSql("uuid_generate_v4()");
                entity.HasQueryFilter(w => !w.IsDeleted);
                entity.HasOne(w => w.Location)
                    .WithOne(l => l.Warehouse)
                    .HasForeignKey<Warehouse>(w => w.Id)
                    .IsRequired();
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(l => l.Id).HasDefaultValueSql("uuid_generate_v4()");
                entity.HasQueryFilter(l => !l.IsDeleted);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Id).HasDefaultValueSql("uuid_generate_v4()");
                entity.HasQueryFilter(u => !u.IsDeleted);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(c => c.Id).HasDefaultValueSql("uuid_generate_v4()");
                entity.HasQueryFilter(c => !c.IsDeleted);
            });

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.Property(p => p.Id).HasDefaultValueSql("uuid_generate_v4()");
                entity.HasQueryFilter(p => !p.IsDeleted);
            });

            modelBuilder.Entity<InventoryMovement>()
                .Property(im => im.Id).HasDefaultValueSql("uuid_generate_v4()");

            modelBuilder.Entity<Sale>()
                .Property(s => s.Id).HasDefaultValueSql("uuid_generate_v4()");

            modelBuilder.Entity<Purchase>()
                .Property(p => p.Id).HasDefaultValueSql("uuid_generate_v4()");

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.Property(rt => rt.Id).HasDefaultValueSql("uuid_generate_v4()");
                entity.HasIndex(rt => rt.Token).IsUnique();
            });

            modelBuilder.Entity<BranchProduct>()
                .HasKey(bp => new { bp.BranchId, bp.ProductId });

            modelBuilder.Entity<WarehouseProduct>()
                .HasKey(wp => new { wp.WarehouseId, wp.ProductId });

            modelBuilder.Entity<BusinessSaleCounter>(entity =>
            {
                entity.HasKey(c => c.BusinessId);
                entity.Property(c => c.Counter).HasDefaultValue(0);
            });

            modelBuilder.Entity<AuditHistory>()
                .Ignore(a => a.Business);

            base.OnModelCreating(modelBuilder);
        }
    }
}
