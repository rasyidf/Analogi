using System.Text.RegularExpressions;
using Analogi.Core.Interfaces;

namespace Analogi.Core.Languages;

public sealed partial class PythonProfile : ILanguageProfile
{
    public string Name => "Python";
    public string[] FileExtensions => [".py"];

    public Regex SingleLineComment => SingleLine();
    public Regex MultiLineComment { get; } = new(
        "(\"\"\"[\\s\\S]*?\"\"\"|'''[\\s\\S]*?''')", RegexOptions.Compiled);
    public Regex FunctionDeclaration => FuncDecl();
    public Regex ImportStatement => Import();

    [GeneratedRegex(@"#[^\n]*", RegexOptions.Compiled)]
    private static partial Regex SingleLine();

    [GeneratedRegex(@"def\s+(?<name>\w+)\s*\(", RegexOptions.Compiled)]
    private static partial Regex FuncDecl();

    [GeneratedRegex(@"(?:from\s+\S+\s+)?import\s+(?<file>\S+)", RegexOptions.Compiled)]
    private static partial Regex Import();
}
