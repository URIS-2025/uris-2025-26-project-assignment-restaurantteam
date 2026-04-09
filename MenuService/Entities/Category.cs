using System.ComponentModel.DataAnnotations;

namespace MenuService.Entities
{
    public class Category
    {
        [Key]
        public int IdCategory { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }

        public List<MenuItemCategory> MenuItemCategories { get; set; } = new List<MenuItemCategory>();


    }
}
