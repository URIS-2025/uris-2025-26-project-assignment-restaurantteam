namespace AccountService.Models
{
    public class MenuItemIngredient
    {
        public int IdMenuItem { get; set; }
        public MenuItem MenuItem { get; set; }

        public int IdIngredient { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
