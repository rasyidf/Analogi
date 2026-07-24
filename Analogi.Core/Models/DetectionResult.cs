namespace Analogi.Core.Models;

public sealed record SimilarityReason(
    string AnalyzerName,
    string Description,
    double Score,
    double Weight = 1.0);

public sealed class DetectionResult
{
    public CodeFile File { get; }
    public List<SimilarityReason> Reasons { get; } = [];

    /// <summary>Weighted average of all reason scores (0..1).</summary>
    public double SimilarityIndex { get; private set; }

    /// <summary>Percentage 0..100.</summary>
    public int SimilarityPercent => (int)(SimilarityIndex * 100);

    public PlagiarismLevel Level => SimilarityIndex switch
    {
        >= 0.90 => PlagiarismLevel.Extreme,
        >= 0.80 => PlagiarismLevel.VeryHigh,
        >= 0.70 => PlagiarismLevel.High,
        >= 0.60 => PlagiarismLevel.Moderate,
        >= 0.40 => PlagiarismLevel.Low,
        >= 0.20 => PlagiarismLevel.Minor,
        _ => PlagiarismLevel.Original,
    };

    public DetectionResult(CodeFile file) => File = file;

    public void AddReasons(IEnumerable<SimilarityReason> reasons)
    {
        Reasons.AddRange(reasons);
        Recalculate();
    }

    private void Recalculate()
    {
        if (Reasons.Count == 0) { SimilarityIndex = 0; return; }
        double totalWeight = Reasons.Sum(r => r.Weight);
        SimilarityIndex = totalWeight == 0 ? 0 : Reasons.Sum(r => r.Score * r.Weight) / totalWeight;
    }
}

public enum PlagiarismLevel
{
    Original = 0,
    Minor = 1,
    Low = 2,
    Moderate = 3,
    High = 4,
    VeryHigh = 5,
    Extreme = 6,
}
