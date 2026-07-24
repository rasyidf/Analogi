using Analogi.Core.Interfaces;
using Analogi.Core.Models;

namespace Analogi.Core.PreProcessors;

/// <summary>
/// Lowercases all metadata values (except file paths) to normalize comparison.
/// </summary>
public sealed class CaseFold : IPipelineStep
{
    public string Name => "CaseFold";
    public bool IsEnabled { get; set; } = true;

    public PipelineContext Run(PipelineContext ctx)
    {
        foreach (var key in ctx.Metadata.Keys.ToList())
        {
            ctx.Metadata[key] = ctx.Metadata[key].ConvertAll(s => s.ToLowerInvariant());
        }
        return ctx;
    }
}

/// <summary>
/// Normalizes whitespace: trims lines, collapses internal whitespace.
/// </summary>
public sealed class WhitespaceNormalize : IPipelineStep
{
    public string Name => "WhitespaceNormalize";
    public bool IsEnabled { get; set; } = true;

    public PipelineContext Run(PipelineContext ctx)
    {
        foreach (var key in ctx.Metadata.Keys.ToList())
        {
            ctx.Metadata[key] = ctx.Metadata[key]
                .ConvertAll(s => NormalizeWhitespace(s))
                .Where(s => s.Length > 0)
                .ToList();
        }
        return ctx;
    }

    private static string NormalizeWhitespace(string s) =>
        string.Join(' ', s.Split(default(char[]), StringSplitOptions.RemoveEmptyEntries));
}
