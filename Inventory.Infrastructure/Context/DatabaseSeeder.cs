using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Context;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(InventoryDbContext context)
    {
        if (await context.Categories.AnyAsync())
            return;

        var locations = new List<Location>
        {
            new() { Id = 1, Address = "123 Main St", City = "New York", CoordinateX = 40.7128, CoordinateY = -74.0060 },
            new() { Id = 2, Address = "456 Oak Ave", City = "Los Angeles", CoordinateX = 34.0522, CoordinateY = -118.2437 },
            new() { Id = 3, Address = "789 Pine Rd", City = "Chicago", CoordinateX = 41.8781, CoordinateY = -87.6298 }
        };
        context.Locations.AddRange(locations);

        var categories = new List<Category>
        {
            new() { Name = "Electronics", Description = "Electronic devices and accessories" },
            new() { Name = "Clothing", Description = "Apparel and fashion items" },
            new() { Name = "Home & Garden", Description = "Home improvement and garden supplies" },
            new() { Name = "Sports", Description = "Sports equipment and gear" }
        };
        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        var products = new List<Product>
        {
            new() { Name = "Laptop Pro", Description = "High-performance laptop", Code = "LAP-001", CategoryId = categories[0].Id },
            new() { Name = "Wireless Mouse", Description = "Ergonomic wireless mouse", Code = "MOU-001", CategoryId = categories[0].Id },
            new() { Name = "Cotton T-Shirt", Description = "Comfortable cotton t-shirt", Code = "TSH-001", CategoryId = categories[1].Id },
            new() { Name = "Denim Jeans", Description = "Classic denim jeans", Code = "JNS-001", CategoryId = categories[1].Id },
            new() { Name = "Garden Tools Set", Description = "Complete garden tool set", Code = "GAR-001", CategoryId = categories[2].Id },
            new() { Name = "Yoga Mat", Description = "Non-slip yoga mat", Code = "YOG-001", CategoryId = categories[3].Id }
        };
        context.Products.AddRange(products);
        await context.SaveChangesAsync();

        var warehouses = new List<Warehouse>
        {
            new() { Name = "Main Warehouse", LocationId = locations[0].Id },
            new() { Name = "West Coast Warehouse", LocationId = locations[1].Id }
        };
        context.Warehouses.AddRange(warehouses);
        await context.SaveChangesAsync();

        var branches = new List<Branch>
        {
            new() { Name = "Downtown Store", LocationId = locations[0].Id },
            new() { Name = "Uptown Store", LocationId = locations[2].Id }
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
                    Stock = new Random().Next(5, 50)
                });
            }
        }
        context.BranchProducts.AddRange(branchProducts);

        var users = new List<User>
        {
            new() { UserName = "admin", Email = "admin@inventory.com", Password = "hashed_password_here", Name = "Administrator", Role = "Admin" },
            new() { UserName = "manager", Email = "manager@inventory.com", Password = "hashed_password_here", Name = "Manager", Role = "Manager" }
        };
        context.Users.AddRange(users);

        await context.SaveChangesAsync();
    }
}
