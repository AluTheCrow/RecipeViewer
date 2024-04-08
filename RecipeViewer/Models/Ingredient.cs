using System.ComponentModel.DataAnnotations;

namespace RecipeViewer.Models;

public class Ingredient
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public double Quantity { get; set; }
    public string? Unit { get; set; }
    public Guid RecipeId { get; set; }
}