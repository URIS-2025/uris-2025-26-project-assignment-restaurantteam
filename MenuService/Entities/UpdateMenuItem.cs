using System.ComponentModel.DataAnnotations;

namespace MenuService.Entities
{
    public class UpdateMenuItem
    {

        [Required]
        [MaxLength(200)]
        public string MenuItemName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 10000)]
        public decimal Price { get; set; }

        [Range(0, 5000)]
        public int Calories { get; set; }

        public bool IsAvailable { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<int> IngredientIds { get; set; }
    }
}
