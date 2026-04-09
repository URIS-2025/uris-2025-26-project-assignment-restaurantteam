using System.ComponentModel.DataAnnotations;

namespace MenuService.DTO.Category
{
    public class CreateCategoryDTO
    {
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }
    }
}
