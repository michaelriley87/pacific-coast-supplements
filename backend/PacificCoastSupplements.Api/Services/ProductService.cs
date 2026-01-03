using Microsoft.EntityFrameworkCore;
using PacificCoastSupplements.Api.Data;
using PacificCoastSupplements.Api.DTOs;
using PacificCoastSupplements.Api.Exceptions;
using PacificCoastSupplements.Api.Interfaces;
using PacificCoastSupplements.Api.Models;

namespace PacificCoastSupplements.Api.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repo;
        private readonly ApplicationDbContext _context;

        public ProductService(IProductRepository repo, ApplicationDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<IEnumerable<ProductReadDto>> GetAllAsync()
        {
            var products = await _repo.GetAllAsync();

            return products.Select(p => new ProductReadDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                CategoryName = p.Category?.Name ?? "",
                Variants = p.Variants.Select(v => new ProductVariantReadDto
                {
                    ProductVariantId = v.ProductVariantId,
                    Size = v.Size,
                    Flavor = v.Flavor,
                    Price = v.Price,
                    Stock = v.Stock
                }).ToList()
            }).ToList();
        }

        public async Task<ProductReadDto> GetByIdAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null)
                throw new NotFoundException($"Product {id} was not found.");

            return new ProductReadDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                CategoryName = p.Category?.Name ?? "",
                Variants = p.Variants.Select(v => new ProductVariantReadDto
                {
                    ProductVariantId = v.ProductVariantId,
                    Size = v.Size,
                    Flavor = v.Flavor,
                    Price = v.Price,
                    Stock = v.Stock
                }).ToList()
            };
        }

        public async Task<ProductReadDto> CreateAsync(ProductCreateDto dto)
        {
            if (dto.Variants == null || dto.Variants.Count == 0)
                throw new BadRequestException("A product must have at least one variant.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new BadRequestException("Product name is required.");

            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == dto.CategoryId);
            if (!categoryExists)
                throw new BadRequestException($"CategoryId {dto.CategoryId} does not exist.");

            var product = new Product
            {
                Name = dto.Name.Trim(),
                Description = dto.Description?.Trim() ?? string.Empty,
                CategoryId = dto.CategoryId
            };

            await _repo.AddAsync(product);
            await _repo.SaveChangesAsync();

            var variants = dto.Variants.Select(v => new ProductVariant
            {
                ProductId = product.ProductId,
                Size = v.Size?.Trim() ?? string.Empty,
                Flavor = v.Flavor?.Trim() ?? string.Empty,
                Price = v.Price,
                Stock = v.Stock
            }).ToList();

            await _context.ProductVariants.AddRangeAsync(variants);
            await _context.SaveChangesAsync();

            var created = await _repo.GetByIdAsync(product.ProductId);
            if (created == null)
                throw new Exception("Failed to reload created product.");

            return new ProductReadDto
            {
                ProductId = created.ProductId,
                Name = created.Name,
                Description = created.Description,
                CategoryName = created.Category?.Name ?? "",
                Variants = created.Variants.Select(v => new ProductVariantReadDto
                {
                    ProductVariantId = v.ProductVariantId,
                    Size = v.Size,
                    Flavor = v.Flavor,
                    Price = v.Price,
                    Stock = v.Stock
                }).ToList()
            };
        }

        public async Task UpdateAsync(int id, ProductUpdateDto dto)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                throw new NotFoundException($"Product {id} was not found.");

            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == dto.CategoryId);
            if (!categoryExists)
                throw new BadRequestException($"CategoryId {dto.CategoryId} does not exist.");

            product.Name = dto.Name.Trim();
            product.Description = dto.Description?.Trim() ?? string.Empty;
            product.CategoryId = dto.CategoryId;

            _repo.Update(product);

            var saved = await _repo.SaveChangesAsync();
            if (!saved)
                throw new Exception("Failed to update product.");
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                throw new NotFoundException($"Product {id} was not found.");

            _repo.Delete(product);

            var saved = await _repo.SaveChangesAsync();
            if (!saved)
                throw new Exception("Failed to delete product.");
        }
    }
}
