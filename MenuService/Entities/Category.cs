using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuService.Entities
{
    public class Category
    {
        public int IdCategory { get; private set; }
        public string CategoryName { get; private set; }

        private Category() { }

        public Category(string name)
        {
            CategoryName = name;
        }
    }
}