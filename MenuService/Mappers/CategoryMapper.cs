using MenuService.DTO.Category;

namespace MenuService.Mappers
{
    public static class CategoryMapper
    {
        public static Entities.Category ToCategory(CategoryDTO categoryDTO)
        {
            return new Entities.Category
            { 
                IdCategory  = categoryDTO.IdCategory,
                CategoryName = categoryDTO.CategoryName
            };

        }
        public static CategoryDTO ToCategoryDTO(Entities.Category category)
        {
            return new CategoryDTO
            {
                IdCategory = category.IdCategory,
                CategoryName = category.CategoryName
            };

        }

        public static CategoryDTO ToCategoryDTO(CreateCategoryDTO createCategoryDTO)
        {
            return new CategoryDTO
            {
                CategoryName = createCategoryDTO.CategoryName
            };

        }

        public static Entities.Category ToCategory(CreateCategoryDTO createCategoryDTO)
        {
            return new Entities.Category
            {
                CategoryName = createCategoryDTO.CategoryName
            };

        }
    }
}
