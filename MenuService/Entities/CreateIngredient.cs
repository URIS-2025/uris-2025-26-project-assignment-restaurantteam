using System.ComponentModel.DataAnnotations;

namespace MenuService.Entities
{
    public class CreateIngredient
    {
        [Required]
        [MaxLength(100)]
        public string IngredientName { get; set; }

        public bool IsAllergen { get; set; }
    }
}
