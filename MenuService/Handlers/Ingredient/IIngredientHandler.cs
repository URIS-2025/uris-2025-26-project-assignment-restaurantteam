using MenuService.DTO.Ingredient;

namespace MenuService.Handlers.Ingredient
{
    public interface IIngredientHandler
    {
        public Task<IngredientDTO> CreateIngredient(CreateIngredientDTO createIngredientDTO);
        public Task<List<IngredientDTO>> GetIngredients();
        public Task<IngredientDTO> GetIngredientById(int idIngredient);
        public Task<IngredientDTO> UpdateIngredient(UpdateIngredientDTO updateIngredientDTO, int idIgredient);
        public Task<bool> DeleteIngredient(int idIngredient);
    }
}
