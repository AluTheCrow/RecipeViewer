using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using RecipeViewer.Data;
using RecipeViewer.Data.IndexedDb;
using RecipeViewer.Models;

namespace RecipeViewer.Pages;

public partial class RecipeView : ComponentBase
{
    [Parameter] public string? Id { get; set; } = String.Empty;

    [Inject] private IDbContextFactory<AppDbContext> DbContextFactory { get; init; }
    [Inject] private IFileUploadRepository FileUploadRepository { get; init; }

    private Recipe? _recipe = new();
    private Guid _recipeId;

    protected override async Task OnInitializedAsync()
    {
        if (!Guid.TryParse(Id, out var id))
        {
        }
        _recipeId = id;

        await using var dbContext = await DbContextFactory.CreateDbContextAsync();

        _recipe = await dbContext.Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Instructions)
            .FirstOrDefaultAsync(r => r.Id == _recipeId);
    }
}