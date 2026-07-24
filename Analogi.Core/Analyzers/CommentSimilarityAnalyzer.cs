using Analogi.Core.Algorithm;
using Analogi.Core.Interfaces;
using Analogi.Core.Models;

namespace Analogi.Core.Analyzers;

/// <summary>
/// Computes cosine similarity on comment text.
/// Students often copy code but forget to change comments.
/// </summary>
public sealed class CommentSimilarityAnalyzer : IPipelineStep
{
    public string Name => "CommentSimilarity";
    public bool IsEnabled { get; set; } = true;
    private const double Threshold = 0.7;

    public PipelineContext Run(PipelineContext ctx)
    {
        var commentsA = ctx.GetMetadata("comment.a");
        var commentsB = ctx.GetMetadata("comment.b");

        if (commentsA.Count == 0 || commentsB.Count == 0) return ctx;

        var textA = string.Join("\n", commentsA);
        var textB = string.Join("\n", commentsB);

        double score = CosineSimilarity.Compute(textA, textB);
        if (score > Threshold)
        {
            ctx.Reasons.Add(new SimilarityReason(
                Name,
                $"Comment similarity: {score:P0}",
                score,
                Weight: 0.4));
        }
        return ctx;
    }
}
