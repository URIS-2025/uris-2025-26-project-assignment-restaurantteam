using System.ComponentModel.DataAnnotations;

namespace MenuService.Entities
{
    public class Ingredient
    {
        [Key]
        public int IdIngredient { get; set; }

        [Required]
        [MaxLength(100)]
        public string IngredientName { get; set; }

        public bool IsAllergen { get; set; }

        public List<MenuItemIngredient> MenuItemIngredients { get; set; } = new List<MenuItemIngredient>();

    }
}
