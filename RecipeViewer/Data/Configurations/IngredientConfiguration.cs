using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeViewer.Models;

namespace RecipeViewer.Data.Configurations;

internal sealed class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.ToTable(DbConstants.IngredientTableName);
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasConversion(c => c.ToString("D"), g => Guid.Parse(g));
        builder.Property(i => i.Name).IsRequired();
        builder.Property(i => i.Quantity)
            .HasConversion(s => s.ToString(), d => Double.Parse(d))
            .IsRequired();
        builder.Property(i => i.Unit).IsRequired(false);

        builder.HasData(
            [
                new() { RecipeId = DbConstants.SampleRecipeId, Name = "Flour", Quantity = 2, Unit = "cups" },
                new() { RecipeId = DbConstants.SampleRecipeId, Name = "Sugar", Quantity = 1, Unit = "cup" },
                new() { RecipeId = DbConstants.SampleRecipeId, Name = "Chocolate Chips", Quantity = 1, Unit = "cup" },
                new() { RecipeId = DbConstants.SampleRecipeId, Name = "Butter", Quantity = 1, Unit = "cup" },
                new() { RecipeId = DbConstants.SampleRecipeId,  Name = "Eggs", Quantity = 2 }
            ]
        );
    }
}