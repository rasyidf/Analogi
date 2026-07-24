using System.Text.RegularExpressions;
using Analogi.Core.Interfaces;
using Analogi.Core.Models;

namespace Analogi.Core.PreProcessors;

/// <summary>
/// Normalizes identifiers to generic tokens (v0, v1, v2...).
/// Defeats alpha-renaming plagiarism (renaming variables to disguise copies).
/// Only applied to code metadata; preserves structure metadata.
/// </summary>
public sealed partial class IdentifierNormalize : IPipelineStep
{
    public string Name => "IdentifierNormalize";

    // ponytail: keywords are language-specific; this uses a common superset.
    // Upgrade path: pull keywords from ILanguageProfile if per-language accuracy is needed.
    private static readonly HashSet<string> Keywords = new(StringComparer.OrdinalIgnoreCase)
    {
        // C/C++/C#/Java shared
        "if", "else", "for", "while", "do", "switch", "case", "break", "continue", "return",
        "void", "int", "float", "double", "char", "bool", "long", "short", "string",
        "class", "struct", "enum", "interface", "namespace", "using", "import", "include",
        "public", "private", "protected", "static", "const", "final", "abstract", "virtual",
        "new", "delete", "this", "null", "true", "false", "try", "catch", "throw", "finally",
        "var", "let", "async", "await", "yield", "export", "default", "from", "require",
        // Python
        "def", "elif", "except", "lambda", "pass", "raise", "with", "as", "in", "not", "and", "or",
        "print", "self", "None", "True", "False",
        // Common types
        "unsigned", "signed", "auto", "override", "readonly", "volatile",
        "main", "cout", "cin", "endl", "std", "iostream", "printf", "scanf",
    };

    public PipelineContext Run(PipelineContext ctx)
    {
        // Only normalize code lines, not comments/structure
        foreach (var key in ctx.Metadata.Keys.Where(k => k.StartsWith("code.")).ToList())
        {
            var mapping = new Dictionary<string, string>();
            int counter = 0;
            ctx.Metadata[key] = ctx.Metadata[key]
                .ConvertAll(line => IdentifierPattern().Replace(line, m =>
                {
                    var id = m.Value;
                    if (Keywords.Contains(id)) return id; // preserve keywords
                    if (!mapping.TryGetValue(id, out var replacement))
                    {
                        replacement = $"v{counter++}";
                        mapping[id] = replacement;
                    }
                    return replacement;
                }));
        }
        return ctx;
    }

    [GeneratedRegex(@"\b[a-zA-Z_]\w*\b", RegexOptions.Compiled)]
    private static partial Regex IdentifierPattern();
}
