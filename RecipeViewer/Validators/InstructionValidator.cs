using FluentValidation;
using RecipeViewer.Models;

namespace RecipeViewer.Validators;

public class InstructionValidator : AbstractValidator<Instruction>
{
    public InstructionValidator()
    {
        RuleFor(instruction => instruction.Title)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(instruction => instruction.Description)
            .NotEmpty()
            .MaximumLength(500);
        RuleFor(instruction => instruction.Order)
            .GreaterThan(0);
    }
}