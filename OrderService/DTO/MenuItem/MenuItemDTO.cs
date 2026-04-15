using System.ComponentModel.DataAnnotations;
using OrderService.DTO.Category;
using OrderService.DTO.Ingredient;

namespace OrderService.DTO.MenuItem
{
    public class MenuItemDTO
    {
        [Key]
        public int IdMenuItem { get; set; }

        [Required]
        [MaxLength(200)]
        public string MenuItemName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }


        [Range(0, 10000)]
        public decimal Price { get; set; }

        [Range(0, 5000)]
        public int Calories { get; set; }

        public bool IsAvailable { get; set; }

        public List<CategoryDTO> Categories { get; set; }
        public List<IngredientDTO> Ingredients { get; set; }


    }
}
