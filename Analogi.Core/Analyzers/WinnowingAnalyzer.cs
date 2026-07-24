using Analogi.Core.Algorithm;
using Analogi.Core.Interfaces;
using Analogi.Core.Models;

namespace Analogi.Core.Analyzers;

/// <summary>
/// Winnowing-based fingerprint analysis (MOSS-style).
/// Creates document fingerprints via the winnowing algorithm, then compares
/// fingerprint sets with Jaccard similarity.
/// Catches code reordering, insertion, and local edits that cosine misses.
/// </summary>
public sealed class WinnowingAnalyzer : IPipelineStep
{
    public string Name => "Winnowing";
    public bool IsEnabled { get; set; } = true;
    private const int KgramSize = 5;
    private const int WindowSize = 4;
    private const double Threshold = 0.3;

    public PipelineContext Run(PipelineContext ctx)
    {
        var codeA = ctx.GetMetadata("code.a");
        var codeB = ctx.GetMetadata("code.b");

        if (codeA.Count < KgramSize || codeB.Count < KgramSize) return ctx;

        var fpA = Winnowing.Fingerprint(codeA, KgramSize, WindowSize);
        var fpB = Winnowing.Fingerprint(codeB, KgramSize, WindowSize);

        double similarity = Winnowing.Similarity(fpA, fpB);

        if (similarity > Threshold)
        {
            int shared = fpA.Count(h => fpB.Contains(h));
            ctx.Reasons.Add(new SimilarityReason(
                Name,
                $"Winnowing fingerprint overlap: {similarity:P0} ({shared} shared fingerprints)",
                similarity,
                Weight: 0.9));
        }
        return ctx;
    }
}
