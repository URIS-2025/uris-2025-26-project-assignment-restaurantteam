using MenuService.DTO.Ingredient;
using MenuService.Handlers.Ingredient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuService.Controllers
{
    [ApiController]
    [Route("api/menu/ingredients")]
    //[Authorize]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientHandler ingredientHandler;
        public IngredientController(IIngredientHandler ingredientHandler)
        {
            this.ingredientHandler = ingredientHandler;
        }

        [HttpPost]
        public async Task<ActionResult<IngredientDTO>> CreateIngredient([FromBody] CreateIngredientDTO createIngredientDTO)
        {
            var ingredientDTO = await ingredientHandler.CreateIngredient(createIngredientDTO);
            return Ok(ingredientDTO);
        }

        [HttpGet]
        public async Task<ActionResult<List<IngredientDTO>>> GetIngredients()
        {
            List<IngredientDTO> ingredients = await ingredientHandler.GetIngredients();
            return Ok(ingredients);
        }

        [HttpGet("{idIngredient}")]
        public async Task<ActionResult<IngredientDTO>> GetIngredientById([FromRoute] int idIngredient)
        {
            IngredientDTO ingredient = await ingredientHandler.GetIngredientById(idIngredient);
            return Ok(ingredient);
        }

        [HttpPut("{idIngredient}")]
        public async Task<ActionResult<IngredientDTO>> UpdateIngredient([FromRoute] int idIngredient, [FromBody] UpdateIngredientDTO updateIngredientDTO)
        {
            IngredientDTO ingredient = await ingredientHandler.UpdateIngredient(updateIngredientDTO, idIngredient);
            return Ok(ingredient);
        }
    }
}
