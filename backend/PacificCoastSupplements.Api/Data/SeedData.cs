using Microsoft.EntityFrameworkCore;
using PacificCoastSupplements.Api.Models;

namespace PacificCoastSupplements.Api.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            if (context.Products.Any())
                return;

            var protein = new Category { Name = "Protein Powders" };
            var creatine = new Category { Name = "Creatine" };
            var accessories = new Category { Name = "Accessories" };

            context.Categories.AddRange(protein, creatine, accessories);
            context.SaveChanges();

            var whey = new Product
            {
                Name = "Whey Protein",
                Description = "High quality whey protein powder.",
                CategoryId = protein.CategoryId
            };

            var mono = new Product
            {
                Name = "Creatine Monohydrate",
                Description = "Micronized creatine monohydrate. Unflavored.",
                CategoryId = creatine.CategoryId
            };

            var shaker = new Product
            {
                Name = "Shaker Bottle",
                Description = "600ml BPA-free shaker bottle.",
                CategoryId = accessories.CategoryId
            };

            context.Products.AddRange(whey, mono, shaker);
            context.SaveChanges();

            context.ProductVariants.AddRange(
                new ProductVariant { ProductId = whey.ProductId, Size = "1kg", Flavor = "Chocolate", Price = 49.99m, Stock = 25 },
                new ProductVariant { ProductId = whey.ProductId, Size = "1kg", Flavor = "Vanilla", Price = 49.99m, Stock = 10 },
                new ProductVariant { ProductId = whey.ProductId, Size = "2kg", Flavor = "Chocolate", Price = 89.99m, Stock = 5 },
                new ProductVariant { ProductId = mono.ProductId, Size = "250g", Flavor = "Unflavored", Price = 29.99m, Stock = 40 },
                new ProductVariant { ProductId = mono.ProductId, Size = "500g", Flavor = "Unflavored", Price = 44.99m, Stock = 20 },
                new ProductVariant { ProductId = shaker.ProductId, Size = "Standard", Flavor = "", Price = 9.99m, Stock = 50 }
            );

            context.SaveChanges();
        }
    }
}
