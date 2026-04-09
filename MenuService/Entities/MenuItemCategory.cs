using System.ComponentModel.DataAnnotations;

namespace MenuService.Entities
{
    public class MenuItemCategory
    {
        public int IdMenuItem { get; set; }
        public MenuItem MenuItem { get; set; }
        public int IdCategory { get; set; }
        public Category Category { get; set; }
    }
}
