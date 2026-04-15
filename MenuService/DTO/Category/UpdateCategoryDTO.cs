using System.ComponentModel.DataAnnotations;

namespace MenuService.DTO.Category
{
    public class UpdateCategoryDTO
    {
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }
    }
}
