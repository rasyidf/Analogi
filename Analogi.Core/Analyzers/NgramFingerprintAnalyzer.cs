using Analogi.Core.Interfaces;
using Analogi.Core.Models;

namespace Analogi.Core.Analyzers;

/// <summary>
/// N-gram fingerprinting: creates overlapping token subsequences and measures
/// Jaccard overlap. Catches code reordering that cosine similarity misses.
/// ponytail: uses simple hash-set Jaccard, not full winnowing. Upgrade path:
/// implement proper winnowing with window selection for better noise filtering.
/// </summary>
public sealed class NgramFingerprintAnalyzer : IPipelineStep
{
    public string Name => "NgramFingerprint";
    private const int NgramSize = 5;
    private const double Threshold = 0.4;

    public PipelineContext Run(PipelineContext ctx)
    {
        var codeA = ctx.GetMetadata("code.a");
        var codeB = ctx.GetMetadata("code.b");

        if (codeA.Count < NgramSize || codeB.Count < NgramSize) return ctx;

        var ngramsA = BuildNgrams(codeA);
        var ngramsB = BuildNgrams(codeB);

        int union = ngramsA.Union(ngramsB).Count();
        if (union == 0) return ctx;

        int intersection = ngramsA.Intersect(ngramsB).Count();
        double jaccard = (double)intersection / union;

        if (jaccard > Threshold)
        {
            ctx.Reasons.Add(new SimilarityReason(
                Name,
                $"N-gram fingerprint overlap: {jaccard:P0} ({intersection} shared {NgramSize}-grams)",
                jaccard,
                Weight: 0.8));
        }
        return ctx;
    }

    private static HashSet<string> BuildNgrams(List<string> lines)
    {
        var ngrams = new HashSet<string>();
        for (int i = 0; i <= lines.Count - NgramSize; i++)
        {
            // Join N consecutive lines as one fingerprint
            var ngram = string.Join("\n", lines.Skip(i).Take(NgramSize));
            ngrams.Add(ngram);
        }
        return ngrams;
    }
}
