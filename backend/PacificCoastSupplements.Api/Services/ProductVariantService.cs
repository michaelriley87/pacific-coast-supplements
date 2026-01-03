using PacificCoastSupplements.Api.DTOs;
using PacificCoastSupplements.Api.Exceptions;
using PacificCoastSupplements.Api.Interfaces;
using PacificCoastSupplements.Api.Models;

namespace PacificCoastSupplements.Api.Services
{
    public class ProductVariantService
    {
        private readonly IProductVariantRepository _repo;

        public ProductVariantService(IProductVariantRepository repo)
        {
            _repo = repo;
        }

        public async Task<ProductVariantReadDto?> GetByIdAsync(int id)
        {
            var v = await _repo.GetByIdAsync(id);
            if (v == null) return null;

            return new ProductVariantReadDto
            {
                ProductVariantId = v.ProductVariantId,
                Size = v.Size,
                Flavor = v.Flavor,
                Price = v.Price,
                Stock = v.Stock
            };
        }

        public async Task<ProductVariantReadDto> CreateAsync(ProductVariantCreateDto dto)
        {
            if (dto.ProductId is null)
                throw new BadRequestException("ProductId is required when creating a variant directly.");

            var variant = new ProductVariant
            {
                ProductId = dto.ProductId.Value,
                Size = dto.Size.Trim(),
                Flavor = dto.Flavor?.Trim() ?? string.Empty,
                Price = dto.Price,
                Stock = dto.Stock
            };

            await _repo.AddAsync(variant);
            await _repo.SaveChangesAsync();

            return new ProductVariantReadDto
            {
                ProductVariantId = variant.ProductVariantId,
                Size = variant.Size,
                Flavor = variant.Flavor,
                Price = variant.Price,
                Stock = variant.Stock
            };
        }

        public async Task UpdateAsync(int id, ProductVariantUpdateDto dto)
        {
            var variant = await _repo.GetByIdAsync(id);
            if (variant == null)
                throw new NotFoundException($"Variant {id} was not found.");

            variant.Size = dto.Size.Trim();
            variant.Flavor = dto.Flavor?.Trim() ?? string.Empty;
            variant.Price = dto.Price;
            variant.Stock = dto.Stock;

            _repo.Update(variant);

            var saved = await _repo.SaveChangesAsync();
            if (!saved)
                throw new Exception("Failed to update variant.");
        }

        public async Task DeleteAsync(int id)
        {
            var variant = await _repo.GetByIdAsync(id);
            if (variant == null)
                throw new NotFoundException($"Variant {id} was not found.");

            _repo.Delete(variant);

            var saved = await _repo.SaveChangesAsync();
            if (!saved)
                throw new Exception("Failed to delete variant.");
        }
    }
}
