using System.Collections.ObjectModel;
using Analogi.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Analogi.App.ViewModels.Pages;

public partial class CompareViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _headerA = string.Empty;

    [ObservableProperty]
    private string _headerB = string.Empty;

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

    [ObservableProperty]
    private bool _isSubmissionMode;

    [ObservableProperty]
    private FilePairResult? _selectedFilePair;

    /// <summary>For submission mode: all file pairs that matched between two submissions.</summary>
    public ObservableCollection<FilePairResult> FilePairs { get; } = [];

    /// <summary>Callback to navigate back to results.</summary>
    public Action? OnBackRequested { get; set; }

    [RelayCommand]
    private void GoBack() => OnBackRequested?.Invoke();

    public void LoadFilePair(FilePairResult pair)
    {
        IsSubmissionMode = false;
        FilePairs.Clear();
        HeaderA = pair.FileA.Name;
        HeaderB = pair.FileB.Name;
        FileAContent = pair.FileA.Content;
        FileBContent = pair.FileB.Content;
        SimilarityText = $"{pair.SimilarityPercent}% similarity ({pair.Level})";
        ReasonsText = FormatReasons(pair.Reasons);
        HasComparison = true;
    }

    public void LoadSubmissionPair(SubmissionPairResult pair)
    {
        IsSubmissionMode = true;
        FilePairs.Clear();

        HeaderA = pair.SubmissionA.Name;
        HeaderB = pair.SubmissionB.Name;
        SimilarityText = $"{pair.SimilarityPercent}% similarity ({pair.Level}) — {pair.FilePairDetails.Count} file pair(s) matched";
        ReasonsText = FormatReasons(pair.Reasons);

        foreach (var fp in pair.FilePairDetails.OrderByDescending(p => p.SimilarityIndex))
            FilePairs.Add(fp);

        // Auto-select the best match
        SelectedFilePair = FilePairs.FirstOrDefault();
        HasComparison = true;
    }

    partial void OnSelectedFilePairChanged(FilePairResult? value)
    {
        if (value == null) return;
        FileAContent = value.FileA.Content;
        FileBContent = value.FileB.Content;
    }

    private static string FormatReasons(List<SimilarityReason> reasons) =>
        string.Join("\n", reasons.Select(r => $"• [{r.AnalyzerName}] {r.Description} (weight: {r.Weight})"));
}
