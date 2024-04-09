using BlazorDexie.Extensions;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Blazorise.LoadingIndicator;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IO;
using RecipeViewer;
using RecipeViewer.Data;
using RecipeViewer.Data.IndexedDb;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;

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
    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
    builder.Services
        .AddBlazorise(options =>
        {
            options.Immediate = true;
            options.ProductToken = "CjxRBHF6NQw9WwJ5fjI1BlEAc3o1CjxXAXl8MQs4bjoNJ2ZdYhBVCCo/CThVPUsNalV8Al44B2ECAWllMit3cWhZPUsCbFtpDUMkGnxIaVliJClwVG0RPUsRWnxNN3EGHEx8Uzx9ABZaZ14sZxIRWgJCLG8NB0hxWDA9SxFaeVk3fwIBSGhAJmQEEVp1TTtvHhxKb188b3sASmdAKn0IGlY1BjxvAgZEalgwbx4DRGBTPGIOGVZnU1l+DhFJcUEqZBJDD2dTL3kSGlNxSTRvHgNEYFM8Yg4ZVmdTWX4OEUlxQSpkEkMPZ1M3YgQLU3FJNG8eA0RgUzxiDhlWZ1NZfg4RSXFBKmQSQw9zfjAEGX1NTmE5YQwlVX1rVV9zfFNrZFVbdRp0XGYIaBJ+TlBBLWUgejVIPzRHIH5xXEJWUSICbRd/JF93AzNtdCtSBnpkb0tSeXI0UGl/ElgoCk4LNBF4JAVcTltRWzZ5Zhd8FlExL2BtWxt+MDtgWlVXZTYHNHA5W1kPPkpaNC5hcAVyXEABYyYCYm17D14gNDNuZxdYIwNBdWYVWzN/XWBdFXwHCipCPRAN";
        })
        .AddBootstrap5Components()
        .AddBootstrap5Providers()
        .AddFontAwesomeIcons()
        .AddLoadingIndicator();

    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");

    builder.Services.AddDbContextFactory<AppDbContext>(options =>
            options
                .UseSqlite($"Filename={DbConstants.RecipeDbName}")
#if DEBUG
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
#endif
    );
    builder.Services.AddDexieWrapper();
    builder.Services.AddScoped<RecipeImageDb>();
    builder.Services.AddScoped<IFileUploadRepository, FileUploadRepository>();
    await builder.Build().RunAsync();
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