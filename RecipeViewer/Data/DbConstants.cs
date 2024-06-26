﻿using System.Runtime.CompilerServices;

namespace RecipeViewer.Data;

public static class DbConstants
{
    public const string RecipeTableName = "Recipes";
    public const string IngredientTableName = "Ingredients";
    public const string InstructionTableName = "Instructions";
    public const string RecipeDbName = "RecipeViewer.db";

    public const string ModuleFileName = "./scripts/indexedDBInterop.js";

    public static readonly Guid SampleRecipeId = Guid.NewGuid();
}