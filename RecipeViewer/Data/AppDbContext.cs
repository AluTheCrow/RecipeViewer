using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using RecipeViewer.Data.Configurations;
using RecipeViewer.Models;

namespace RecipeViewer.Data;

internal sealed class AppDbContext(DbContextOptions<AppDbContext> options, IJSRuntime jsRuntime) : DbContext(options)
{
    public DbSet<Recipe> Recipes { get; set; } = default!;
    public DbSet<Ingredient> Ingredients { get; set; } = default!;
    public DbSet<Instruction> Instructions { get; set; } = default!;

    private readonly Lazy<Task<IJSObjectReference>> _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
        "import", DbConstants.ModuleFileName).AsTask());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecipeConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var result = await base.SaveChangesAsync(cancellationToken);
#if RELEASE
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("syncDbFileToIndexedDb", false, cancellationToken);
#endif
        return result;
    }

}
