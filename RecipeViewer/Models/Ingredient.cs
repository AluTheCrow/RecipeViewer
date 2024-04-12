using System.ComponentModel.DataAnnotations;

namespace RecipeViewer.Models;

public class Ingredient
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Name { get; set; }
    [Required]
    public double Quantity { get; set; }
    public string? Unit { get; set; }
    public Guid RecipeId { get; set; }
}