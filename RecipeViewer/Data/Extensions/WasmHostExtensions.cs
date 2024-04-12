using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace RecipeViewer.Data.Extensions;

public static class WasmHostExtensions
{
    public static Task InitializeDbAsync(this WebAssemblyHost host)
    {
        var dbService = host.Services.GetRequiredService<RecipeViewerDbService<AppDbContext>>();
        return dbService.CreateDatabaseAsync();
    }
}