using Microsoft.EntityFrameworkCore;
using RecipeViewer.Data.Configurations;
using RecipeViewer.Models;

namespace RecipeViewer.Data;

internal sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Recipe> Recipes { get; set; } = default!;
    public DbSet<Ingredient> Ingredients { get; set; } = default!;
    public DbSet<Instruction> Instructions { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecipeConfiguration).Assembly);
    }
}
