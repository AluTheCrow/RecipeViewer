using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Blazorise.LoadingIndicator;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.IO;
using RecipeViewer;
using RecipeViewer.Data;
using RecipeViewer.Data.Extensions;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;
using System.Diagnostics.CodeAnalysis;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .Enrich.WithMachineName()
    .Enrich.WithThreadName()
    .Enrich.WithProcessName()
    .Enrich.WithMemoryUsage()
    .WriteTo.Async(a =>
    {
        a.Console(theme: AnsiConsoleTheme.Code);
        a.BrowserConsole();
        a.Debug(formatter: new CompactJsonFormatter());
    })
    .CreateLogger();

try
{
    var builder = WebAssemblyHostBuilder.CreateDefault(args);

    builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
    builder.Services.AddSingleton(_ => new RecyclableMemoryStreamManager());
    builder.Services.AddSingleton<RecipeViewerDbService<AppDbContext>>();
    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
    builder.Services
        .AddBlazorise(options =>
        {
            options.Immediate = true;
        })
        .AddBootstrap5Components()
        .AddBootstrap5Providers()
        .AddFontAwesomeIcons()
        .AddLoadingIndicator();

    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");
    builder.Services.AddRecipeDbServices();
    await using var host = builder.Build();
    await host.InitializeDbAsync();
    await host.RunAsync();
}
catch (Exception e)
{
    Log.Fatal(e, "Recipe Viewer failed to launch: {Message}", e.Message);
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}

public partial class Program
{
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    private static Type _keepDateOnly = typeof(DateOnly);
}