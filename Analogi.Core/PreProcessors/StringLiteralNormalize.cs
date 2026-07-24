using System.Text.RegularExpressions;
using Analogi.Core.Interfaces;
using Analogi.Core.Models;

namespace Analogi.Core.PreProcessors;

/// <summary>
/// Replaces all string literals with a placeholder token.
/// Catches plagiarism where only string constants were changed.
/// </summary>
public sealed partial class StringLiteralNormalize : IPipelineStep
{
    public string Name => "StringLiteralNormalize";

    public PipelineContext Run(PipelineContext ctx)
    {
        foreach (var key in ctx.Metadata.Keys.Where(k => k.StartsWith("code.")).ToList())
        {
            ctx.Metadata[key] = ctx.Metadata[key]
                .ConvertAll(line => StringLiteral().Replace(line, "__STR__"));
        }
        return ctx;
    }

    // Matches "...", '...', `...` (with escaped quote handling)
    [GeneratedRegex(@"""(?:[^""\\]|\\.)*""|'(?:[^'\\]|\\.)*'|`(?:[^`\\]|\\.)*`", RegexOptions.Compiled)]
    private static partial Regex StringLiteral();
}
