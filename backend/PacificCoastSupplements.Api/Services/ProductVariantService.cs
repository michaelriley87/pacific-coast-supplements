using PacificCoastSupplements.Api.DTOs;
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
            var variant = new ProductVariant
            {
                Size = dto.Size,
                Flavor = dto.Flavor,
                Price = dto.Price,
                Stock = dto.Stock,
                ProductId = dto.ProductId
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

        public async Task<bool> UpdateAsync(int id, ProductVariantUpdateDto dto)
        {
            var variant = await _repo.GetByIdAsync(id);
            if (variant == null) return false;

            variant.Size = dto.Size;
            variant.Flavor = dto.Flavor;
            variant.Price = dto.Price;
            variant.Stock = dto.Stock;

            _repo.Update(variant);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var variant = await _repo.GetByIdAsync(id);
            if (variant == null) return false;

            _repo.Delete(variant);
            return await _repo.SaveChangesAsync();
        }
    }
}
