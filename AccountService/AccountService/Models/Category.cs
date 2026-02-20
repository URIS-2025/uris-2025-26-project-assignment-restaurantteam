using System.Collections.Generic;

namespace AccountService.Models
{
    public class Category
    {
        public int IdCategory { get; set; }
        public string CategoryName { get; set; }

        public ICollection<MenuItemCategory> MenuItemCategories { get; set; }
    }
}
