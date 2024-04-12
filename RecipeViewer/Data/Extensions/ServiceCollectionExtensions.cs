using BlazorDexie.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RecipeViewer.Data.IndexedDb;

namespace RecipeViewer.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRecipeDbServices(this IServiceCollection services)
    {
#if DEBUG
        services.AddDbContextFactory<AppDbContext>(options =>
        {
            options.UseInMemoryDatabase("RecipeViewerDb");

            options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        });

#else
        services.AddDbContextFactory<AppDbContext>(options =>
            options.UseSqlite($"FileName={RecipeViewerDbService<AppDbContext>.FileName}"));
#endif
        services.AddDexieWrapper();
        services.AddScoped<RecipeImageDb>();
        services.AddScoped<IFileUploadRepository, FileUploadRepository>();

        return services;
    }


}
