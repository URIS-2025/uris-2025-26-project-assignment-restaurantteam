using MenuService.DTO.Category;

namespace MenuService.Handlers.Category
{
    public interface ICategoryHandler
    {
        public Task<CategoryDTO> CreateCategory(CreateCategoryDTO createCategoryDTO);
        public Task<List<CategoryDTO>> GetCategories();
        public Task<CategoryDTO> GetCategoryById(int idCategory);
        public Task<CategoryDTO> UpdateCategory(UpdateCategoryDTO updateCategoryDTO, int idCategory);
        public Task<bool> DeleteCategory(int idCategory);
    }
}
