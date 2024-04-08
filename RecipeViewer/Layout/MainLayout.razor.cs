using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Reflection;

namespace RecipeViewer.Layout;

public partial class MainLayout : LayoutComponentBase
{
    private Bar _sidebar;
    [Inject] private IJSRuntime JSRuntime { get; init; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("initializeImageZoom");
        }
    }
    private static string AssemblyProductVersion
    {
        get
        {
            var attributes = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
            return attributes.Length == 0 ?
                String.Empty :
                ((AssemblyFileVersionAttribute)attributes[0]).Version;
        }
    }

    private static string ApplicationDevelopmentCompany
    {
        get
        {
            var attributes = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            return attributes.Length == 0 ?
                String.Empty :
                ((AssemblyCompanyAttribute)attributes[0]).Company;
        }
    }
}