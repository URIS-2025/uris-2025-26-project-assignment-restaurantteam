namespace MenuService.Entities
{
    public class MenuItemResponse
    {
        public int IdMenuItem { get; set; }
        public string MenuItemName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Calories { get; set; }
        public bool IsAvailable { get; set; }
        public List<Category> Categories { get; set; }

        // Sastojci za prikaz
        public List<Ingredient> Ingredients { get; set; }
    }
}
