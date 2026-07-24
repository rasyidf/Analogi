using Analogi.Core.Interfaces;
using Analogi.Core.Models;

namespace Analogi.Core.Analyzers;

/// <summary>
/// Compares function names between files. High overlap suggests copied structure.
/// </summary>
public sealed class StructureAnalyzer : IPipelineStep
{
    public string Name => "StructureSimilarity";
    public bool IsEnabled { get; set; } = true;
    private const double Threshold = 0.5;

    public PipelineContext Run(PipelineContext ctx)
    {
        var funcsA = ctx.GetMetadata("functions.a");
        var funcsB = ctx.GetMetadata("functions.b");

        if (funcsA.Count == 0 && funcsB.Count == 0) return ctx;

        var setA = new HashSet<string>(funcsA, StringComparer.OrdinalIgnoreCase);
        var setB = new HashSet<string>(funcsB, StringComparer.OrdinalIgnoreCase);

        int union = setA.Union(setB).Count();
        if (union == 0) return ctx;

        int intersection = setA.Intersect(setB).Count();
        double jaccard = (double)intersection / union;

        if (jaccard > Threshold)
        {
            ctx.Reasons.Add(new SimilarityReason(
                Name,
                $"Function name overlap (Jaccard): {jaccard:P0}",
                jaccard,
                Weight: 0.5));
        }
        return ctx;
    }
}
