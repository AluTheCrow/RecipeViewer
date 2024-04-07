using FluentValidation;
using RecipeViewer.Models;

namespace RecipeViewer.Validators;

public class RecipeValidator : AbstractValidator<Recipe>
{
    public RecipeValidator()
    {

        RuleFor(recipe => recipe.Title)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(recipe => recipe.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleForEach(recipe => recipe.Ingredients)
            .SetValidator(new IngredientValidator());

        RuleForEach(recipe => recipe.Instructions)
            .SetValidator(new InstructionValidator());
    }
}