using Microsoft.AspNetCore.Components;
using RecipeViewer.Models;

namespace RecipeViewer.Pages;

public partial class RecipeView : ComponentBase
{
    [Parameter] public Guid? Id { get; set; } = Guid.Empty;
    private Recipe _recipe = new();

    protected override void OnInitialized()
    {
        _recipe = new()
        {
            Id = Guid.NewGuid(),
            Title = "Chocolate Chip Cookies",
            Description = "A delicious recipe for chocolate chip cookies.",
            ImageUrl = "https://www.example.com/chocolate-chip-cookies.jpg",
            Ingredients = new List<Ingredient>
            {
                new() { Name = "Flour", Quantity = 2, Unit = "cups" },
                new() { Name = "Sugar", Quantity = 1, Unit = "cup" },
                new() { Name = "Chocolate Chips", Quantity = 1, Unit = "cup" },
                new() { Name = "Butter", Quantity = 1, Unit = "cup" },
                new() { Name = "Eggs", Quantity = 2 }
            },
            Instructions = new List<Instruction>
            {
                new() { Order = 1, Description = "Preheat oven to 350°F." },
                new() { Order = 2, Description = "Mix flour, sugar, and chocolate chips in a bowl." },
                new() { Order = 3, Description = "Add butter and eggs to the bowl and mix well." },
                new() { Order = 4, Description = "Drop spoonfuls of dough onto a baking sheet." },
                new() { Order = 5, Description = "Bake for 10-12 minutes or until golden brown." }
            }
        };
    }
}