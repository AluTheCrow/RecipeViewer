namespace RecipeViewer.Models;

public class Instruction
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
}