using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using RecipeViewer.Data;
using RecipeViewer.Data.IndexedDb;
using RecipeViewer.Models;

namespace RecipeViewer.Components;

public partial class EditRecipeModal : ComponentBase
{
    [Parameter] public Guid RecipeId { get; set; }

    [Inject] private IDbContextFactory<AppDbContext> _dbContextFactory { get; init; }
    [Inject] private IToastService ToastService { get; init; }
    [Inject] private IModalService ModalService { get; init; }
    [Inject] private ILogger<EditRecipeModal> Logger { get; init; }
    [Inject] private IFileUploadRepository FileUploadRepository { get; init; }

    private Recipe? _recipe;

    protected override async Task OnInitializedAsync()
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        _recipe = await dbContext.Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Instructions)
            .FirstOrDefaultAsync(r => r.Id == RecipeId);

        await base.OnInitializedAsync();
    }

    private async Task SaveRecipeAsync()
    {
        if (_recipe is null)
        {
            return;
        }

        try
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            dbContext.Recipes.Update(_recipe);
            await dbContext.SaveChangesAsync();
            await ToastService.Success("Recipe saved successfully.");
            await CloseModalAsync();
        }
        catch (Exception e)
        {
            await ToastService.Error(new MarkupString($"<small>{e.Message}</small>"), "Error updating recipe.");
            Logger.LogError(e, "An error occured while updating this recipe: {Message}", e.Message);
        }
    }

    private Task CloseModalAsync() => ModalService.Hide();
}