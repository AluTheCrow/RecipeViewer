using System.Security.Cryptography.X509Certificates;
using Blazorise;
using Blazorise.DataGrid;
using Blazorise.LoadingIndicator;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.IO;
using RecipeViewer.Data;
using RecipeViewer.Data.IndexedDb;
using RecipeViewer.Models;

namespace RecipeViewer.Components;

public partial class RecipeModal : ComponentBase
{
    private const int MaxChunkSize = 24576;
    [Inject] private ILoadingIndicatorService LoadingIndicatorService { get; init; }
    [Inject] private IModalService ModalService { get; init; }
    [Inject] private IToastService ToastService { get; init; }
    [Inject] private IDbContextFactory<AppDbContext> DbContextFactory { get; init; }
    [Inject] private IFileUploadRepository FileUploadRepository { get; init; }
    [Inject] private ILogger<RecipeModal> Logger { get; init; }
    [Parameter] public string Title { get; set; } = "New Recipe";

    private DataGrid<Instruction> _instructionDataGrid;
    private DataGrid<Ingredient> _ingredientDataGrid;

    private Steps _stepsRef;
    private string _selectedStep = "1";
    private string _markdownValue = "";
    private Recipe _recipe = new();
    private Ingredient _selectedIngredient;
    private Instruction _selectedInstruction;
    private readonly List<Ingredient> _ingredients = [];
    private readonly List<Instruction> _instructions = [];
    private bool _isLocalImage;
    private async Task SaveAsync()
    {
        await LoadingIndicatorService.Show();
        _recipe.Description = _markdownValue;
        _recipe.Ingredients = _ingredients;
        _recipe.Instructions = _instructions;
        await using var dbContext = await DbContextFactory.CreateDbContextAsync();

        dbContext.Recipes.Add(_recipe);
        await dbContext.SaveChangesAsync();
        await ModalService.Hide();
        await LoadingIndicatorService.Hide();
    }

    private async Task OnFileUploadAsync(FileUploadEventArgs e)
    {
        try
        {
            await LoadingIndicatorService.Show();

            Logger.LogInformation("Uploading file {FileName} with {Length} bytes", e.File.Name, e.File.Size);
            var uploadUrl = await FileUploadRepository.UploadFileAsync(_recipe.Id, e.File.OpenReadStream(long.MaxValue));
            _recipe.ImageUrl = uploadUrl;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error uploading file");
            await ToastService.Error(new MarkupString($"<p>{ex.Message}"), $"{e.File.Name} failed to upload due to an exception");
        }
        finally
        {
            await LoadingIndicatorService.Hide();
        }
    }

    private Task OnSwitchFlippedAsync(bool value)
    {
        _isLocalImage = value;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private bool CanNavigateTo(StepNavigationContext context) =>
        context is { CurrentStepIndex: <= 3, NextStepIndex: < 4 }
        || (_ingredients.Count > 0 && _instructions.Count > 0);

    private Task CancelAsync() => ModalService.Hide();
}
