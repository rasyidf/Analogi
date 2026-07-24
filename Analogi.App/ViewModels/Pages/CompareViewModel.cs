using Analogi.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Analogi.App.ViewModels.Pages;

public partial class CompareViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _fileAName = string.Empty;

    [ObservableProperty]
    private string _fileBName = string.Empty;

    [ObservableProperty]
    private string _fileAContent = string.Empty;

    [ObservableProperty]
    private string _fileBContent = string.Empty;

    [ObservableProperty]
    private string _similarityText = string.Empty;

    [ObservableProperty]
    private string _reasonsText = string.Empty;

    [ObservableProperty]
    private bool _hasComparison;

    public void LoadFilePair(FilePairResult pair)
    {
        FileAName = pair.FileA.Name;
        FileBName = pair.FileB.Name;
        FileAContent = pair.FileA.Content;
        FileBContent = pair.FileB.Content;
        SimilarityText = $"{pair.SimilarityPercent}% similarity ({pair.Level})";
        ReasonsText = string.Join("\n", pair.Reasons.Select(r =>
            $"• [{r.AnalyzerName}] {r.Description} (weight: {r.Weight})"));
        HasComparison = true;
    }

    public void LoadSubmissionPair(SubmissionPairResult pair)
    {
        FileAName = pair.SubmissionA.Name;
        FileBName = pair.SubmissionB.Name;

        // Show the best matching file pair content
        var bestPair = pair.FilePairDetails.OrderByDescending(p => p.SimilarityIndex).FirstOrDefault();
        if (bestPair != null)
        {
            FileAContent = bestPair.FileA.Content;
            FileBContent = bestPair.FileB.Content;
        }
        else
        {
            FileAContent = "(no matching files)";
            FileBContent = "(no matching files)";
        }

        SimilarityText = $"{pair.SimilarityPercent}% similarity ({pair.Level})";
        ReasonsText = string.Join("\n", pair.Reasons.Select(r =>
            $"• [{r.AnalyzerName}] {r.Description} (weight: {r.Weight})"));
        HasComparison = true;
    }
}
