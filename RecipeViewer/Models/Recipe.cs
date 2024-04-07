namespace RecipeViewer.Models;

public sealed class Recipe
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string Description { get; set; }
    public string? ImageUrl { get; set; }
    public IEnumerable<Ingredient> Ingredients { get; set; } = [];
    public IEnumerable<Instruction> Instructions { get; set; } = [];
}
