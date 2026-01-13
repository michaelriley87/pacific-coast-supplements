using PacificCoastSupplements.Api.Models;

namespace PacificCoastSupplements.Api.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category?> GetByNameAsync(string name);
        Task AddAsync(Category category);
        void Update(Category category);
        void Delete(Category category);
        Task<bool> SaveChangesAsync();
        Task<bool> ExistsAsync(int id);
    }
}
