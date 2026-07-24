using Analogi.Core.Algorithm;
using Analogi.Core.Interfaces;
using Analogi.Core.Models;

namespace Analogi.Core.Analyzers;

/// <summary>
/// Computes cosine similarity between the code of both files.
/// </summary>
public sealed class CosineSimilarityAnalyzer : IPipelineStep
{
    public string Name => "CosineSimilarity";
    public bool IsEnabled { get; set; } = true;
    private const double Threshold = 0.6;

    public PipelineContext Run(PipelineContext ctx)
    {
        var codeA = string.Join("\n", ctx.GetMetadata("code.a"));
        var codeB = string.Join("\n", ctx.GetMetadata("code.b"));

        double score = CosineSimilarity.Compute(codeA, codeB);
        if (score > Threshold)
        {
            ctx.Reasons.Add(new SimilarityReason(
                Name,
                $"Code cosine similarity: {score:P0}",
                score,
                Weight: 1.0));
        }
        return ctx;
    }
}
