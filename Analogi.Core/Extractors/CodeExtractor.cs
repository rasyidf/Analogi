using Analogi.Core.Interfaces;
using Analogi.Core.Models;

namespace Analogi.Core.Extractors;

/// <summary>
/// Extracts code lines (with comments removed) from both files.
/// </summary>
public sealed class CodeExtractor : IPipelineStep
{
    public string Name => "CodeExtractor";
    public bool IsEnabled { get; set; } = true;

    public PipelineContext Run(PipelineContext ctx)
    {
        ctx.SetMetadata("code.a", Extract(ctx.FileA.Content, ctx.Language));
        ctx.SetMetadata("code.b", Extract(ctx.FileB.Content, ctx.Language));
        return ctx;
    }

    private static List<string> Extract(string content, ILanguageProfile lang)
    {
        var stripped = lang.MultiLineComment.Replace(content, "");
        return stripped
            .Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries)
            .Select(line => lang.SingleLineComment.Replace(line, "").Trim())
            .Where(line => line.Length > 0)
            .ToList();
    }
}
