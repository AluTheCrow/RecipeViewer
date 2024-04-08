using System.Runtime.InteropServices;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using RecipeViewer.Components;
using RecipeViewer.Data;
using RecipeViewer.Models;

namespace RecipeViewer.Pages;

public partial class Home : ComponentBase
{
    public const string ModuleFileName = "./scripts/indexedDBInterop.js";

    private string _version = "unknown";

    private readonly List<RecipeListViewModel> _recipes = [];
    [Inject] private IModalService ModalService { get; init; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; init; } = default!;
    [Inject] private IDbContextFactory<AppDbContext> _dbContextFactory { get; init; }

    protected override async Task OnInitializedAsync()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("browser")))
        {
            await using var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./scripts/indexedDbInterop.js");
            await module.InvokeVoidAsync("syncDbFileToIndexedDb", DbConstants.RecipeDbName);
        }

        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        await dbContext.Database.EnsureCreatedAsync();

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

    private async Task SQLiteVersion()
    {
        await using var db = new SqliteConnection($"Data Source={DbConstants.RecipeDbName}");
        await db.OpenAsync();
        await using var cmd = new SqliteCommand("SELECT SQLITE_VERSION()", db);
        _version = (await cmd.ExecuteScalarAsync())?.ToString();
    }

    private Task AddRecipeAsync()
    {
        return ModalService.Show<RecipeModal>("Add Recipe");
    }
}