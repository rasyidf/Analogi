namespace Analogi.Core.Models;

/// <summary>
/// Represents a student submission — one or more source files in a directory.
/// </summary>
public sealed class Submission
{
    public string Name { get; }
    public string DirectoryPath { get; }
    public IReadOnlyList<CodeFile> Files { get; }

    /// <summary>All file content concatenated (for whole-submission comparison).</summary>
    public string CombinedContent { get; }

    public long TotalSize => Files.Sum(f => f.Length);
    public int TotalLines => Files.Sum(f => f.Content.Split('\n').Length);

    public Submission(string directoryPath, IReadOnlyList<CodeFile> files)
    {
        DirectoryPath = directoryPath;
        Name = Path.GetFileName(directoryPath);
        Files = files;
        CombinedContent = string.Join("\n", files.Select(f => f.Content));
    }
}

/// <summary>
/// Result for a submission pair comparison.
/// </summary>
public sealed class SubmissionPairResult
{
    public Submission SubmissionA { get; }
    public Submission SubmissionB { get; }
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

    /// <summary>Per-file pair details (which specific files matched).</summary>
    public List<FilePairResult> FilePairDetails { get; }

    public SubmissionPairResult(Submission a, Submission b, List<SimilarityReason> reasons, List<FilePairResult> filePairDetails)
    {
        SubmissionA = a;
        SubmissionB = b;
        Reasons = reasons;
        FilePairDetails = filePairDetails;

        if (reasons.Count == 0) { SimilarityIndex = 0; return; }
        double totalWeight = reasons.Sum(r => r.Weight);
        SimilarityIndex = totalWeight == 0 ? 0 : reasons.Sum(r => r.Score * r.Weight) / totalWeight;
    }
}

/// <summary>
/// Complete output of a submission-mode scan.
/// </summary>
public sealed class SubmissionScanResult
{
    public IReadOnlyList<SubmissionPairResult> Pairs { get; }
    public int TotalSubmissions { get; }
    public TimeSpan Duration { get; }

    public SubmissionScanResult(IReadOnlyList<SubmissionPairResult> pairs, int totalSubmissions, TimeSpan duration)
    {
        Pairs = pairs;
        TotalSubmissions = totalSubmissions;
        Duration = duration;
    }
}
