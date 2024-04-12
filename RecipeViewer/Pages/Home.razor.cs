using System.Runtime.InteropServices;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using RecipeViewer.Components;
using RecipeViewer.Data;
using RecipeViewer.Data.IndexedDb;
using RecipeViewer.Models;

namespace RecipeViewer.Pages;

public partial class Home : ComponentBase
{
    private readonly List<RecipeListViewModel> _recipes = [];
    [Inject] private IModalService ModalService { get; init; } = default!;
    [Inject] private IMessageService MessageService { get; init; } = default!;
    [Inject] private IToastService ToastService { get; init; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; init; } = default!;
    [Inject] private ILogger<Home> Logger { get; init; } = default!;
    [Inject] private IFileUploadRepository FileUploadRepository { get; init; } = default!;
    [Inject] private IDbContextFactory<AppDbContext> _dbContextFactory { get; init; }

    protected override async Task OnInitializedAsync()
    {
        _recipes.Clear();

        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var storedRecipes = await dbContext.Recipes
            .Select(r => new RecipeListViewModel
            (
                r.Id,
                r.Title,
                r.Description,
                r.Instructions.Count()
            ))
            .ToListAsync();

        _recipes.AddRange(storedRecipes);
    }

    private Task AddRecipeAsync()
    {
        return ModalService.Show<RecipeModal>("Add Recipe");
    }

    private Task EditRecipeAsync(string name, Guid id)
    {
        return ModalService.Show<EditRecipeModal>($"Edit {name} Recipe", parameters => parameters.Add(x => x.RecipeId, id));
    }

    private async Task DeleteRecipeAsync(Guid id)
    {
        if (!await MessageService.Confirm("Are you sure you want to delete this recipe?"))
        {
            StateHasChanged();
            return;
        }
        var recipe = await RemoveRecipeFromDbAsync(id);

        if (recipe is null)
        {
            return;
        }

        await RemoveRecipeFromIndexedDb(recipe);

        await ToastService.Success(new MarkupString($"Recipe {recipe.Title} removed"), "Success Deleting Recipe");

        await InvokeAsync(StateHasChanged);
    }

    private async Task<Recipe?> RemoveRecipeFromDbAsync(Guid id)
    {
        try
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            var recipe = await dbContext.Recipes.FirstOrDefaultAsync(r => r.Id == id);

            dbContext.Recipes.Remove(recipe);

            await dbContext.SaveChangesAsync();

            _recipes.RemoveAll(r => r.Id == id);

            return recipe;
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error removing recipe from database: {Message}", e.Message);
            await ToastService.Error(new MarkupString($"<small>{e.Message}</small>"), "Error Deleting Recipe");
            return null;
        }
    }

    private ValueTask<bool> RemoveRecipeFromIndexedDb(Recipe recipe) => FileUploadRepository.DeleteBlobAsync(recipe.Id, recipe.ImageUrl, CancellationToken.None);

}