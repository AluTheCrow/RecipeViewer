using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace RecipeViewer.Data;

internal sealed class RecipeViewerDbService<T>(ILogger<RecipeViewerDbService<T>> logger)
    where T : DbContext
{
#if RELEASE
    public static string FileName { get; } = $"/database/{DbConstants.RecipeDbName}";
    private readonly IDbContextFactory<T> _dbContextFactory;
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
    public RecipeViewerDbService(IDbContextFactory<T> dbContextFactory, IJSRuntime jsRuntime, ILogger<RecipeViewerDbService<T>> logger)
        : this(logger)
    {
        ArgumentNullException.ThrowIfNull(dbContextFactory, nameof(dbContextFactory));
        ArgumentNullException.ThrowIfNull(jsRuntime, nameof(jsRuntime));
        _dbContextFactory = dbContextFactory;
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", DbConstants.ModuleFileName).AsTask());
    }
#endif
    public async Task CreateDatabaseAsync()
    {
        try
        {
#if RELEASE
            var module = await _moduleTask.Value;
            await module.InvokeVoidAsync("mountAndInitializeSqliteFile", FileName);

            if (!File.Exists(FileName))
            {
                File.Create(FileName).Close();
            }

            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            await dbContext.Database.EnsureCreatedAsync();
#endif
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating database: {Message}", e.Message);
        }
    }

}