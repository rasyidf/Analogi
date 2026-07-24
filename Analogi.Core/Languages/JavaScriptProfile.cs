using System.Text.RegularExpressions;
using Analogi.Core.Interfaces;

namespace Analogi.Core.Languages;

public sealed partial class JavaScriptProfile : ILanguageProfile
{
    public string Name => "JavaScript/TypeScript";
    public string[] FileExtensions => [".js", ".jsx", ".ts", ".tsx", ".mjs", ".cjs"];

    public Regex SingleLineComment => SingleLine();
    public Regex MultiLineComment => MultiLine();
    public Regex FunctionDeclaration => FuncDecl();
    public Regex ImportStatement => Import();

    [GeneratedRegex(@"//[^\n]*", RegexOptions.Compiled)]
    private static partial Regex SingleLine();

    [GeneratedRegex(@"/\*[\s\S]*?\*/", RegexOptions.Compiled)]
    private static partial Regex MultiLine();

    // Matches: function name(, const name = (, name(, export function name(, async function name(
    [GeneratedRegex(@"(?:export\s+)?(?:async\s+)?(?:function\s+(?<name>\w+)|(?:const|let|var)\s+(?<name>\w+)\s*=\s*(?:async\s*)?\(?)", RegexOptions.Compiled)]
    private static partial Regex FuncDecl();

    [GeneratedRegex(@"(?:import\s+(?:.*?\s+from\s+)?['""](?<file>[^'""]+)['""]|require\s*\(\s*['""](?<file>[^'""]+)['""]\s*\))", RegexOptions.Compiled)]
    private static partial Regex Import();
}
