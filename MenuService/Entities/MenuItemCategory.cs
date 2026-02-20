using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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