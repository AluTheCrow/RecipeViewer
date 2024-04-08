using System.Security.Cryptography.X509Certificates;
using Blazorise;
using Blazorise.DataGrid;
using Blazorise.LoadingIndicator;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using RecipeViewer.Data;
using RecipeViewer.Models;

namespace RecipeViewer.Components;

public partial class RecipeModal : ComponentBase
{
    [Inject] private ILoadingIndicatorService LoadingIndicatorService { get; init; }
    [Inject] private IModalService ModalService { get; init; }
    [Inject] private IDbContextFactory<AppDbContext> DbContextFactory { get; init; }
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

    private bool CanNavigateTo(StepNavigationContext context) =>
        context is { CurrentStepIndex: <= 3, NextStepIndex: < 4 }
        || (_ingredients.Count > 0 && _instructions.Count > 0);

    private Task CancelAsync() => ModalService.Hide();
}
