namespace RecipeViewer.Models;

public sealed class Recipe
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string Description { get; set; }
    public string? ImageUrl { get; set; }
    public ICollection<Ingredient> Ingredients { get; set; } = [];
    public ICollection<Instruction> Instructions { get; set; } = [];
}
