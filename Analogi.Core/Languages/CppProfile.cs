using System.Text.RegularExpressions;
using Analogi.Core.Interfaces;

namespace Analogi.Core.Languages;

public sealed partial class CppProfile : ILanguageProfile
{
    public string Name => "C/C++";
    public string[] FileExtensions => [".c", ".cpp", ".cc", ".cxx", ".h", ".hpp"];

    public Regex SingleLineComment => SingleLine();
    public Regex MultiLineComment => MultiLine();
    public Regex FunctionDeclaration => FuncDecl();
    public Regex ImportStatement => Include();

    [GeneratedRegex(@"//[^\n]*", RegexOptions.Compiled)]
    private static partial Regex SingleLine();

    [GeneratedRegex(@"/\*[\s\S]*?\*/", RegexOptions.Compiled)]
    private static partial Regex MultiLine();

    [GeneratedRegex(@"(?:(?:int|void|bool|float|double|char|long|short|unsigned|auto|string|vector)\s+)+(?<name>\w+)\s*\([^)]*\)\s*\{?", RegexOptions.Compiled)]
    private static partial Regex FuncDecl();

    [GeneratedRegex(@"#\s*include\s*[<""](?<file>[^>""]+)[>""]", RegexOptions.Compiled)]
    private static partial Regex Include();
}
