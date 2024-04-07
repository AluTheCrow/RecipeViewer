using Blazorise;
using Microsoft.AspNetCore.Components;
using RecipeViewer.Models;

namespace RecipeViewer.Components;

public partial class RecipeModal : ComponentBase
{
    [Inject] private IModalService ModalService { get; set; }

    [Parameter] public string Title { get; set; } = "New Recipe";

    private string _markdownValue = "";
    private string _markdownAsHtml;
    private Recipe _recipe = new();
    private Ingredient _selectedIngredient;
    private Instruction _selectedInstruction;
    private readonly List<Ingredient> _ingredients = [];
    private readonly List<Instruction> _instructions = [];

    private async Task SaveAsync()
    {
        await ModalService.Hide();
    }

    private async Task CancelAsync()
    {
        await ModalService.Hide();
    }

    private Task OnMarkdownValueChangedAsync(string value)
    {
        _markdownValue = value;
        _markdownAsHtml = Markdig.Markdown.ToHtml(value);
        return Task.CompletedTask;
    }

    private Task<string> PreviewRenderAsync(string plainText)
    => Task.FromResult(Markdig.Markdown.ToHtml(plainText));
}
