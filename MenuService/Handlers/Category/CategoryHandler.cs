using MenuService.Data;
using MenuService.DTO.Category;
using MenuService.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MenuService.Handlers.Category
{
    public class CategoryHandler : ICategoryHandler
    {
        private readonly MenuDbContext _context;

        public CategoryHandler(MenuDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryDTO> CreateCategory(CreateCategoryDTO createCategoryDTO)
        {
            var existCategory = await _context.Categories.FirstOrDefaultAsync(u =>
               u.CategoryName == createCategoryDTO.CategoryName
            );

            if (existCategory != null)
                throw new Exception("Category exists!");

            var category = CategoryMapper.ToCategory(createCategoryDTO);

             _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var categoryDTO = CategoryMapper.ToCategoryDTO(category);
            categoryDTO.IdCategory = category.IdCategory;
            return categoryDTO;
        }

        public async Task<bool> DeleteCategory(int idCategory)
        {
            Entities.Category? category = await _context.Categories.FindAsync(idCategory);

            if (category == null)
            {
                throw new Exception("Category not found");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;

            
        }

        public async Task<List<CategoryDTO>> GetCategories()
        {
            return _context.Categories
                .Select(CategoryMapper.ToCategoryDTO)
                .ToList();
        }

        public async Task<CategoryDTO> GetCategoryById(int idCategory)
        {
            Entities.Category? category = await _context.Categories.FindAsync(idCategory);

            if (category == null)
            {
                throw new Exception("Category not found");

            }

            return CategoryMapper.ToCategoryDTO(category);
        }

        public async Task<CategoryDTO> UpdateCategory(UpdateCategoryDTO updateCategoryDTO, int idCategory)
        {
            Entities.Category? category = await _context.Categories.FindAsync(idCategory);

            if (category == null)
            {
                throw new Exception("Category not found");
            }

            category.CategoryName = updateCategoryDTO.CategoryName;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            CategoryDTO categoryDTO = CategoryMapper.ToCategoryDTO(category);

            return categoryDTO;
        }
    }
}
