using System.ComponentModel.DataAnnotations;

namespace RecipeViewer.Models;

public class Instruction
{
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    public int Order { get; set; }
    public Guid RecipeId { get; set; }
}