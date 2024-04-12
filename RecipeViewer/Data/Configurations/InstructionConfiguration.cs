using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeViewer.Models;

namespace RecipeViewer.Data.Configurations;

internal sealed class InstructionConfiguration : IEntityTypeConfiguration<Instruction>
{
    public void Configure(EntityTypeBuilder<Instruction> builder)
    {
        builder.ToTable(DbConstants.InstructionTableName);
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasConversion(c => c.ToString("D"), g => Guid.Parse(g));
        builder.Property(i => i.Order).IsRequired();
        builder.Property(i => i.Description).IsRequired();

        builder.HasData(
                       [new()
                       {
                            Title= "Preheat",
                            RecipeId = DbConstants.SampleRecipeId,
                            Order = 1,
                            Description = "Preheat oven to 350°F."
                       },
                       new()
                       {
                           Title = "Mix Dry ingredients",
                            RecipeId = DbConstants.SampleRecipeId,
                            Order = 2,
                            Description = "Mix flour, sugar, and chocolate chips in a bowl."
                       },
                       new()
                       {
                           Title = "Mix in Wet ingredients",
                            RecipeId = DbConstants.SampleRecipeId,
                            Order = 3,
                            Description = "Add butter and eggs to the bowl and mix well."
                       },
                       new()
                       {
                           Title = "Spread dough evenly",
                            RecipeId = DbConstants.SampleRecipeId,
                            Order = 4,
                            Description = "Drop spoonfuls of dough onto a baking sheet."
                       },
                       new()
                       {
                           Title = "Bake",
                            RecipeId = DbConstants.SampleRecipeId,
                            Order = 5,
                            Description = "Bake for 10-12 minutes or until golden brown."
                       }
                      ]);
    }
}