using FluentValidation;
using RecipeViewer.Models;

namespace RecipeViewer.Validators;

public class IngredientValidator : AbstractValidator<Ingredient>
{
    public IngredientValidator()
    {
        RuleFor(ingredient => ingredient.Name)
            .NotEmpty()
            .WithMessage("The ingredient needs a name!");

        RuleFor(ingredient => ingredient.Quantity)
            .NotEmpty()
            .WithMessage("The ingredient needs a quantity!")
            .GreaterThan(0d)
            .WithMessage("The quantity must be a number greater than 0");
    }
}