namespace RecipeViewer;

public static class CsProjConstantsForDisplay
{
    public const string CsProjContent = """
                                        <PropertyGroup>
                                        		<WasmBuildNative>true</WasmBuildNative>
                                        		<AllowUnsafeBlocks>true</AllowUnsafeBlocks>
                                        		<EmccExtraLDFlags>-s WARN_ON_UNDEFINED_SYMBOLS=0</EmccExtraLDFlags>
                                        		<RunAOTCompilation>true</RunAOTCompilation>
                                        		<WasmStripILAfterAOT>false</WasmStripILAfterAOT>
                                        </PropertyGroup>
                                        """;

    public static readonly string CsProjContentEscaped = System.Security.SecurityElement.Escape(CsProjContent);
}