using System.ComponentModel.DataAnnotations;

namespace MenuService.DTO.Category
{
    public class CategoryDTO
    {
        public int IdCategory { get; set; }
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }
    }
}
