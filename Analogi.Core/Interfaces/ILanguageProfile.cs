using System.Text.RegularExpressions;

namespace Analogi.Core.Interfaces;

/// <summary>
/// Defines how to parse a specific programming language.
/// Implement this to add support for a new language.
/// </summary>
public interface ILanguageProfile
{
    string Name { get; }
    string[] FileExtensions { get; }
    Regex SingleLineComment { get; }
    Regex MultiLineComment { get; }
    Regex FunctionDeclaration { get; }
    Regex ImportStatement { get; }
}
