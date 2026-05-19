using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Context;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(InventoryDbContext context)
    {
        if (await context.Businesses.AnyAsync())
            return;

        var business = new Business { Name = "Inventory Co." };
        context.Businesses.Add(business);
        await context.SaveChangesAsync();

        var locations = new List<Location>
        {
            new() { Address = "123 Main St", City = "New York", CoordinateX = 40.7128, CoordinateY = -74.0060 },
            new() { Address = "456 Oak Ave", City = "Los Angeles", CoordinateX = 34.0522, CoordinateY = -118.2437 },
            new() { Address = "789 Pine Rd", City = "Chicago", CoordinateX = 41.8781, CoordinateY = -87.6298 }
        };
        context.Locations.AddRange(locations);

        var roles = new List<Role>
        {
            new() { Id = 1, Name = "Admin", Description = "Administrator with full access" },
            new() { Id = 2, Name = "Seller", Description = "Seller with read-only access" }
        };
        context.Roles.AddRange(roles);

        var measures = new List<Measure>
        {
            new() { Id = 1, Name = "Unit" },
            new() { Id = 2, Name = "Kilogram" },
            new() { Id = 3, Name = "Liter" },
            new() { Id = 4, Name = "Meter" },
            new() { Id = 5, Name = "Box" },
            new() { Id = 6, Name = "Pack" }
        };
        context.Measures.AddRange(measures);

        var categories = new List<Category>
        {
            new() { Name = "Electronics", Description = "Electronic devices and accessories", BusinessId = business.Id },
            new() { Name = "Clothing", Description = "Apparel and fashion items", BusinessId = business.Id },
            new() { Name = "Home & Garden", Description = "Home improvement and garden supplies", BusinessId = business.Id },
            new() { Name = "Sports", Description = "Sports equipment and gear", BusinessId = business.Id }
        };
        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        var products = new List<Product>
        {
            new() { Name = "Laptop Pro", Description = "High-performance laptop", Code = "LAP-001", CategoryId = categories[0].Id, MeasureId = measures[0].Id, BusinessId = business.Id },
            new() { Name = "Wireless Mouse", Description = "Ergonomic wireless mouse", Code = "MOU-001", CategoryId = categories[0].Id, MeasureId = measures[0].Id, BusinessId = business.Id },
            new() { Name = "Cotton T-Shirt", Description = "Comfortable cotton t-shirt", Code = "TSH-001", CategoryId = categories[1].Id, MeasureId = measures[0].Id, BusinessId = business.Id },
            new() { Name = "Denim Jeans", Description = "Classic denim jeans", Code = "JNS-001", CategoryId = categories[1].Id, MeasureId = measures[0].Id, BusinessId = business.Id },
            new() { Name = "Garden Tools Set", Description = "Complete garden tool set", Code = "GAR-001", CategoryId = categories[2].Id, MeasureId = measures[0].Id, BusinessId = business.Id },
            new() { Name = "Yoga Mat", Description = "Non-slip yoga mat", Code = "YOG-001", CategoryId = categories[3].Id, MeasureId = measures[0].Id, BusinessId = business.Id }
        };
        context.Products.AddRange(products);
        await context.SaveChangesAsync();

        var warehouses = new List<Warehouse>
        {
            new() { Name = "Main Warehouse", Location = locations[0], BusinessId = business.Id },
            new() { Name = "West Coast Warehouse", Location = locations[1], BusinessId = business.Id }
        };
        context.Warehouses.AddRange(warehouses);
        await context.SaveChangesAsync();

        var branches = new List<Branch>
        {
            new() { Name = "Downtown Store", Location = locations[0], BusinessId = business.Id },
            new() { Name = "Uptown Store", Location = locations[2], BusinessId = business.Id }
        };
        context.Branches.AddRange(branches);
        await context.SaveChangesAsync();

        var warehouseProducts = new List<WarehouseProduct>();
        foreach (var warehouse in warehouses)
        {
            foreach (var product in products)
            {
                warehouseProducts.Add(new WarehouseProduct
                {
                    WarehouseId = warehouse.Id,
                    ProductId = product.Id,
                    Stock = new Random().Next(10, 100)
                });
            }
        }
        context.WarehouseProducts.AddRange(warehouseProducts);

        var branchProducts = new List<BranchProduct>();
        foreach (var branch in branches)
        {
            foreach (var product in products)
            {
                branchProducts.Add(new BranchProduct
                {
                    BranchId = branch.Id,
                    ProductId = product.Id,
                    Stock = new Random().Next(5, 50),
                    Price = Math.Round(new Random().NextDouble() * (100 - 10) + 10, 2)
                });
            }
        }
        context.BranchProducts.AddRange(branchProducts);

        var users = new List<User>
        {
            new() { UserName = "admin", Email = "admin@inventory.com", Password = BCrypt.Net.BCrypt.HashPassword("admin123"), Name = "Administrator", RoleId = 1, BusinessId = business.Id },
            new() { UserName = "manager", Email = "manager@inventory.com", Password = BCrypt.Net.BCrypt.HashPassword("manager123"), Name = "Manager", RoleId = 2, BusinessId = business.Id }
        };
        context.Users.AddRange(users);

        context.BusinessSaleCounters.Add(new BusinessSaleCounter { BusinessId = business.Id, Counter = 0 });

        await context.SaveChangesAsync();
    }
}
