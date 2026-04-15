using MenuService.DTO.Ingredient;

namespace MenuService.Mappers
{
    public static class IngredientMapper
    {
        public static Entities.Ingredient ToIngredient(IngredientDTO ingredientDTO)
        {
            return new Entities.Ingredient
            {
                IdIngredient = ingredientDTO.IdIngredient,
                IngredientName = ingredientDTO.IngredientName,
                IsAllergen = ingredientDTO.IsAllergen
            };

        }
        public static IngredientDTO ToIngredientDTO(Entities.Ingredient ingredient)
        {
            return new IngredientDTO
            {
                IdIngredient = ingredient.IdIngredient,
                IngredientName = ingredient.IngredientName,
                IsAllergen = ingredient.IsAllergen
            };

        }

        public static IngredientDTO ToIngredientDTO(CreateIngredientDTO createIngredientDTO)
        {
            return new IngredientDTO
            {
                IngredientName = createIngredientDTO.IngredientName,
                IsAllergen = createIngredientDTO.IsAllergen
            };

        }

        public static Entities.Ingredient ToIngredient(CreateIngredientDTO createIngredientDTO)
        {
            return new Entities.Ingredient
            {
                IngredientName = createIngredientDTO.IngredientName,
                IsAllergen = createIngredientDTO.IsAllergen
            };

        }
    }
}
