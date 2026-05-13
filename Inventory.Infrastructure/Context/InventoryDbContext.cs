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
        public DbSet<Business> Businesss { get; set; }
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Id)
                .UseIdentityByDefaultColumn();
            modelBuilder.Entity<Category>()
                .Property(c => c.Id)
                .UseIdentityByDefaultColumn();
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
            modelBuilder.Entity<Customer>()
                .Property(c => c.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<Purchase>()
                .Property(p => p.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<Provider>()
                .Property(p => p.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<AuditHistory>()
                .Ignore(a => a.User);
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
            modelBuilder.Entity<Customer>()
                .HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Provider>()
                .HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Warehouse>()
                .HasOne(w => w.Location)
                .WithOne(l => l.Warehouse)
                .HasForeignKey<Warehouse>(w => w.Id)
                .IsRequired();
            modelBuilder.Entity<Warehouse>()
                .HasIndex(w => w.Id)
                .IsUnique();
            modelBuilder.Entity<Branch>()
                .HasOne(b => b.Location)
                .WithOne(l => l.Branch)
                .HasForeignKey<Branch>(b => b.Id)
                .IsRequired();
            modelBuilder.Entity<Branch>()
                .HasIndex(b => b.Id)
                .IsUnique();
            modelBuilder.Entity<Category>()
                .HasOne(c => c.Business)
                .WithMany(b => b.Categories)
                .HasForeignKey(c => c.BusinessId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Business)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BusinessId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Warehouse>()
                .HasOne(w => w.Business)
                .WithMany(b => b.Warehouses)
                .HasForeignKey(w => w.BusinessId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Branch>()
                .HasOne(br => br.Business)
                .WithMany(b => b.Branches)
                .HasForeignKey(br => br.BusinessId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Business)
                .WithMany(b => b.Users)
                .HasForeignKey(u => u.BusinessId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Business)
                .WithMany(b => b.Customers)
                .HasForeignKey(c => c.BusinessId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Business)
                .WithMany(b => b.Purchases)
                .HasForeignKey(p => p.BusinessId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Business)
                .WithMany(b => b.Sales)
                .HasForeignKey(s => s.BusinessId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AuditHistory>()
                .HasOne(a => a.Business)
                .WithMany(b => b.AuditHistories)
                .HasForeignKey(a => a.BusinessId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Provider>()
                .HasOne(p => p.Business)
                .WithMany(b => b.Providers)
                .HasForeignKey(p => p.BusinessId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InventoryMovement>()
                .HasOne(im => im.Business)
                .WithMany(b => b.InventoryMovements)
                .HasForeignKey(im => im.BusinessId)
                .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
        }
    }
}
