using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RecipeViewer.Data;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace RecipeViewer.Pages;

public partial class About : ComponentBase
{
    [Inject] private IJSRuntime JSRuntime { get; init; } = default!;
    protected override async Task OnInitializedAsync()
    {

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("browser")))
        {
            await using var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./scripts/codeblockHighlight.js");

            await module.InvokeVoidAsync("applySyntaxHighlightingWithJQuery");
        }
    }

}
