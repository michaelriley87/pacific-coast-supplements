using PacificCoastSupplements.Api.Models;

namespace PacificCoastSupplements.Api.Interfaces
{
    public interface IProductVariantRepository
    {
        Task<ProductVariant?> GetByIdAsync(int id);
        Task AddAsync(ProductVariant variant);
        void Update(ProductVariant variant);
        void Delete(ProductVariant variant);
        Task<bool> SaveChangesAsync();
    }
}
