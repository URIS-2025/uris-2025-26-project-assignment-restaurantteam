using System.ComponentModel.DataAnnotations;

namespace MenuService.DTO.Ingredient
{
    public class IngredientDTO
    {
        public int IdIngredient { get; set; }
        [Required]
        [MaxLength(100)]
        public string IngredientName { get; set; }

        public bool IsAllergen { get; set; }
    }
}
