using MenuService.DTO.Category;
using MenuService.DTO.Ingredient;

namespace MenuService.Handlers.MenuItemIngredient
{
    public interface IMenuItemIngredientHandler
    {
        public Task<List<IngredientDTO>> CreateMenuItemIngredient(int idMenuItem, List<int> ingredientIds);
        public Task<List<IngredientDTO>> GetMenuItemIngredients(int idMenuItem);

        //public Task<bool> DeleteMenuItemIngredients(int idMenuItem);
    }
}
