using Analogi.Core.Interfaces;
using Analogi.Core.Models;

namespace Analogi.Core.Analyzers;

/// <summary>
/// Jaccard similarity on import/include lists.
/// Unusual shared imports suggest a common source.
/// </summary>
public sealed class ImportOverlapAnalyzer : IPipelineStep
{
    public string Name => "ImportOverlap";
    public bool IsEnabled { get; set; } = true;
    private const double Threshold = 0.7;

    public PipelineContext Run(PipelineContext ctx)
    {
        var importsA = ctx.GetMetadata("imports.a");
        var importsB = ctx.GetMetadata("imports.b");

        if (importsA.Count == 0 && importsB.Count == 0) return ctx;

        var setA = new HashSet<string>(importsA, StringComparer.OrdinalIgnoreCase);
        var setB = new HashSet<string>(importsB, StringComparer.OrdinalIgnoreCase);

        int union = setA.Union(setB).Count();
        if (union == 0) return ctx;

        int intersection = setA.Intersect(setB).Count();
        double jaccard = (double)intersection / union;

        if (jaccard > Threshold)
        {
            ctx.Reasons.Add(new SimilarityReason(
                Name,
                $"Import overlap (Jaccard): {jaccard:P0} ({intersection}/{union} shared)",
                jaccard,
                Weight: 0.3));
        }
        return ctx;
    }
}
