using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuService.Entities
{
    public class MenuItem
    {
        public int IdMenuItem { get; private set; }
        public string MenuItemName { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int Calories { get; private set; }
        public bool IsAvailable { get; private set; }

        public ICollection<MenuItemCategory> Categories { get; private set; }
        public ICollection<MenuItemIngredient> Ingredients { get; private set; }

        private MenuItem() { }

        public MenuItem(string name, string description, decimal price, int calories)
        {
            MenuItemName = name;
            Description = description;
            Price = price;
            Calories = calories;
            IsAvailable = true;
            Categories = new List<MenuItemCategory>();
            Ingredients = new List<MenuItemIngredient>();
        }

        public void Deactivate()
        {
            IsAvailable = false;
        }
    }
}