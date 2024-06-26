﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeViewer.Models;

namespace RecipeViewer.Data.Configurations;

internal sealed class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable(DbConstants.RecipeTableName);
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasConversion(c => c.ToString("D"), g => Guid.Parse(g));
        builder.Property(r => r.Title).IsRequired();
        builder.Property(r => r.Description).IsRequired();
        builder.Property(r => r.ImageUrl);

        builder.HasMany(r => r.Ingredients)
            .WithOne()
            .HasForeignKey(i => i.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(r => r.Instructions)
            .WithOne()
            .HasForeignKey(i => i.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData([
            new()
            {
                Id = DbConstants.SampleRecipeId,
                Title = "Chocolate Chip Cookies",
                Description = "A delicious recipe for chocolate chip cookies.",
                ImageUrl = "https://www.example.com/chocolate-chip-cookies.jpg"
            }
        ]);
    }
}