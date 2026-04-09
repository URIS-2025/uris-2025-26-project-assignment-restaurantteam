using System.ComponentModel.DataAnnotations;

namespace MenuService.Entities
{
    public class CreateCategory
    {
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }
    }
}
