using BlazorDexie.Database;
using BlazorDexie.JsModule;

namespace RecipeViewer.Data.IndexedDb;

public sealed class RecipeImageDb(IModuleFactory jsModuleFactory) : Db("RecipeBlobDataDb", 2, new DbVersion[] { new Version1() }, jsModuleFactory, camelCaseStoreNames: true)
{
    public Store<byte[], Guid> RecipeImages { get; set; } = new(string.Empty);
}