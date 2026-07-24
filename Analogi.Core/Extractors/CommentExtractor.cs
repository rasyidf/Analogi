using System.Text.RegularExpressions;
using Analogi.Core.Interfaces;
using Analogi.Core.Models;

namespace Analogi.Core.Extractors;

/// <summary>
/// Extracts comment text from both files.
/// </summary>
public sealed class CommentExtractor : IPipelineStep
{
    public string Name => "CommentExtractor";
    public bool IsEnabled { get; set; } = true;

    public PipelineContext Run(PipelineContext ctx)
    {
        ctx.SetMetadata("comment.a", Extract(ctx.FileA.Content, ctx.Language));
        ctx.SetMetadata("comment.b", Extract(ctx.FileB.Content, ctx.Language));
        return ctx;
    }

    private static List<string> Extract(string content, ILanguageProfile lang)
    {
        var comments = new List<string>();

        foreach (Match m in lang.MultiLineComment.Matches(content))
            comments.Add(m.Value);
        foreach (Match m in lang.SingleLineComment.Matches(content))
            comments.Add(m.Value);

        return comments;
    }
}
