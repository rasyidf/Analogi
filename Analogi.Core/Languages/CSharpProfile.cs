using System.Text.RegularExpressions;
using Analogi.Core.Interfaces;

namespace Analogi.Core.Languages;

public sealed partial class CSharpProfile : ILanguageProfile
{
    public string Name => "C#";
    public string[] FileExtensions => [".cs"];

    public Regex SingleLineComment => SingleLine();
    public Regex MultiLineComment => MultiLine();
    public Regex FunctionDeclaration => FuncDecl();
    public Regex ImportStatement => Import();

    [GeneratedRegex(@"//[^\n]*", RegexOptions.Compiled)]
    private static partial Regex SingleLine();

    [GeneratedRegex(@"/\*[\s\S]*?\*/", RegexOptions.Compiled)]
    private static partial Regex MultiLine();

    [GeneratedRegex(@"(?:public|private|protected|internal|static|async|override|virtual|abstract|\s)+[\w<>\[\]?]+\s+(?<name>\w+)\s*\([^)]*\)\s*\{?", RegexOptions.Compiled)]
    private static partial Regex FuncDecl();

    [GeneratedRegex(@"using\s+(?:static\s+)?(?<file>[\w.]+)\s*;", RegexOptions.Compiled)]
    private static partial Regex Import();
}
