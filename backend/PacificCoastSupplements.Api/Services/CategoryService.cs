using PacificCoastSupplements.Api.DTOs;
using PacificCoastSupplements.Api.Exceptions;
using PacificCoastSupplements.Api.Interfaces;
using PacificCoastSupplements.Api.Models;

namespace PacificCoastSupplements.Api.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<CategoryReadDto>> GetAllAsync()
        {
            var categories = await _repo.GetAllAsync();

            return categories.Select(c => new CategoryReadDto
            {
                CategoryId = c.CategoryId,
                Name = c.Name
            }).ToList();
        }

        public async Task<CategoryReadDto> GetByIdAsync(int id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException($"Category {id} was not found.");

            return new CategoryReadDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name
            };
        }

        public async Task<CategoryReadDto> CreateAsync(CategoryCreateDto dto)
        {
            var name = dto.Name.Trim();

            var existing = await _repo.GetByNameAsync(name);
            if (existing != null)
                throw new BadRequestException($"Category '{name}' already exists.");

            var category = new Category
            {
                Name = name
            };

            await _repo.AddAsync(category);

            var saved = await _repo.SaveChangesAsync();
            if (!saved)
                throw new Exception("Failed to create category.");

            return new CategoryReadDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name
            };
        }

        public async Task UpdateAsync(int id, CategoryUpdateDto dto)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException($"Category {id} was not found.");

            var name = dto.Name.Trim();

            var existing = await _repo.GetByNameAsync(name);
            if (existing != null && existing.CategoryId != id)
                throw new BadRequestException($"Category '{name}' already exists.");

            category.Name = name;

            _repo.Update(category);

            var saved = await _repo.SaveChangesAsync();
            if (!saved)
                throw new Exception("Failed to update category.");
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException($"Category {id} was not found.");

            if (category.Products != null && category.Products.Count > 0)
                throw new BadRequestException("Cannot delete a category that still has products.");

            _repo.Delete(category);

            var saved = await _repo.SaveChangesAsync();
            if (!saved)
                throw new Exception("Failed to delete category.");
        }
    }
}
