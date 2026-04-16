using MenuService.Data;
using MenuService.DTO.Ingredient;
using MenuService.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MenuService.Handlers.Ingredient
{
    public class IngredientHandler : IIngredientHandler
    {
        private readonly MenuDbContext _context;

        public IngredientHandler(MenuDbContext context)
        {
            _context = context;
        }

        public async Task<IngredientDTO> CreateIngredient(CreateIngredientDTO createIngredientDTO)
        {
            var existIngredient = await _context.Ingredients.FirstOrDefaultAsync(u =>
               u.IngredientName == createIngredientDTO.IngredientName
            );

            if (existIngredient != null)
                throw new Exception("Ingredient exists!");

            var ingredient = IngredientMapper.ToIngredient(createIngredientDTO);

            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();

            var ingredientDTO = IngredientMapper.ToIngredientDTO(ingredient);
            ingredientDTO.IdIngredient = ingredient.IdIngredient;

            return ingredientDTO;
        }

        public async Task<bool> DeleteIngredient(int idIngredient)
        {
            Entities.Ingredient? ingredient = await _context.Ingredients.FindAsync(idIngredient);

            if (ingredient == null)
            {
                throw new Exception("Ingredient not found");
            }

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
            return true;


        }

        public async Task<List<IngredientDTO>> GetIngredients()
        {
            return _context.Ingredients
                .Select(IngredientMapper.ToIngredientDTO)
                .ToList();
        }

        public async Task<IngredientDTO> GetIngredientById(int idIngredient)
        {
            Entities.Ingredient? ingredient = await _context.Ingredients.FindAsync(idIngredient);


            if (ingredient == null)
            {
                throw new Exception("Ingredients not found");

            }

            return IngredientMapper.ToIngredientDTO(ingredient);
        }

        public async Task<IngredientDTO> UpdateIngredient(UpdateIngredientDTO updateIngredientDTO, int idIngredient)
        {
            Entities.Ingredient? ingredient = await _context.Ingredients.FindAsync(idIngredient);

            if (ingredient == null)
            {
                throw new Exception("Ingredient not found");
            }

            ingredient.IngredientName = updateIngredientDTO.IngredientName;
            ingredient.IsAllergen = updateIngredientDTO.IsAllergen;


            _context.Ingredients.Update(ingredient);
            await _context.SaveChangesAsync();

            IngredientDTO ingredientDTO = IngredientMapper.ToIngredientDTO(ingredient);

            return ingredientDTO;
        }
    }
}
