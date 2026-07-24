namespace Analogi.Core.Models;

/// <summary>
/// The complete output of a scan operation — one DetectionResult per file pair.
/// </summary>
public sealed class ScanResult
{
    public IReadOnlyList<FilePairResult> Pairs { get; }
    public int TotalFiles { get; }
    public TimeSpan Duration { get; }

    public ScanResult(IReadOnlyList<FilePairResult> pairs, int totalFiles, TimeSpan duration)
    {
        Pairs = pairs;
        TotalFiles = totalFiles;
        Duration = duration;
    }
}

/// <summary>
/// Result for a single pair comparison (A vs B).
/// </summary>
public sealed class FilePairResult
{
    public CodeFile FileA { get; }
    public CodeFile FileB { get; }
    public List<SimilarityReason> Reasons { get; }
    public double SimilarityIndex { get; }
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

    public FilePairResult(CodeFile fileA, CodeFile fileB, List<SimilarityReason> reasons)
    {
        FileA = fileA;
        FileB = fileB;
        Reasons = reasons;
        if (reasons.Count == 0) { SimilarityIndex = 0; return; }
        double totalWeight = reasons.Sum(r => r.Weight);
        SimilarityIndex = totalWeight == 0 ? 0 : reasons.Sum(r => r.Score * r.Weight) / totalWeight;
    }
}
