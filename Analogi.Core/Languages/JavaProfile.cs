using System.Text.RegularExpressions;
using Analogi.Core.Interfaces;

namespace Analogi.Core.Languages;

public sealed partial class JavaProfile : ILanguageProfile
{
    public string Name => "Java";
    public string[] FileExtensions => [".java"];

    public Regex SingleLineComment => SingleLine();
    public Regex MultiLineComment => MultiLine();
    public Regex FunctionDeclaration => FuncDecl();
    public Regex ImportStatement => Import();

    [GeneratedRegex(@"//[^\n]*", RegexOptions.Compiled)]
    private static partial Regex SingleLine();

    [GeneratedRegex(@"/\*[\s\S]*?\*/", RegexOptions.Compiled)]
    private static partial Regex MultiLine();

    [GeneratedRegex(@"(?:public|private|protected|static|\s)+[\w<>\[\]]+\s+(?<name>\w+)\s*\([^)]*\)\s*(?:throws\s+\w+(?:\s*,\s*\w+)*)?\s*\{?", RegexOptions.Compiled)]
    private static partial Regex FuncDecl();

    [GeneratedRegex(@"import\s+(?:static\s+)?(?<file>[\w.]+)\s*;", RegexOptions.Compiled)]
    private static partial Regex Import();
}
