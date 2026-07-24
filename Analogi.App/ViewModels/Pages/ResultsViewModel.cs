using System.Collections.ObjectModel;
using Analogi.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Analogi.App.ViewModels.Pages;

public partial class ResultsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _summaryText = "No scan results yet. Run a scan first.";

    [ObservableProperty]
    private string _filterLabel = "All";

    [ObservableProperty]
    private bool _hasResults;

    public Action? OnBackRequested { get; set; }

    [RelayCommand]
    private void GoBack() => OnBackRequested?.Invoke();

    public ObservableCollection<ResultItemViewModel> FilteredResults { get; } = [];

    private readonly List<ResultItemViewModel> _allResults = [];
    private PlagiarismLevel? _filterLevel;

    public void LoadFileResults(ScanResult result)
    {
        _allResults.Clear();
        foreach (var pair in result.Pairs.OrderByDescending(p => p.SimilarityIndex))
            _allResults.Add(new ResultItemViewModel(pair));

        _filterLevel = null;
        ApplyFilter();
        HasResults = true;
        SummaryText = $"{result.TotalFiles} files scanned, {result.Pairs.Count} similar pairs found.";
    }

    public void LoadSubmissionResults(SubmissionScanResult result)
    {
        _allResults.Clear();
        foreach (var pair in result.Pairs.OrderByDescending(p => p.SimilarityIndex))
            _allResults.Add(new ResultItemViewModel(pair));

        _filterLevel = null;
        ApplyFilter();
        HasResults = true;
        SummaryText = $"{result.TotalSubmissions} submissions scanned, {result.Pairs.Count} similar pairs found.";
    }

    [RelayCommand]
    private void SetFilter(string? level)
    {
        _filterLevel = level switch
        {
            "High" => PlagiarismLevel.High,
            "Moderate" => PlagiarismLevel.Moderate,
            "Low" => PlagiarismLevel.Low,
            _ => null
        };
        FilterLabel = level ?? "All";
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        FilteredResults.Clear();
        foreach (var r in _allResults)
        {
            if (_filterLevel == null || (int)r.Level >= (int)_filterLevel)
                FilteredResults.Add(r);
        }
    }
}

/// <summary>Unified row item for both file pairs and submission pairs.</summary>
public class ResultItemViewModel
{
    private readonly FilePairResult? _filePair;
    private readonly SubmissionPairResult? _subPair;

    public string NameA { get; }
    public string NameB { get; }
    public int SimilarityPercent { get; }
    public PlagiarismLevel Level { get; }
    public int ReasonCount { get; }

    public ResultItemViewModel(FilePairResult pair)
    {
        _filePair = pair;
        NameA = pair.FileA.Name;
        NameB = pair.FileB.Name;
        SimilarityPercent = pair.SimilarityPercent;
        Level = pair.Level;
        ReasonCount = pair.Reasons.Count;
    }

    public ResultItemViewModel(SubmissionPairResult pair)
    {
        _subPair = pair;
        NameA = pair.SubmissionA.Name;
        NameB = pair.SubmissionB.Name;
        SimilarityPercent = pair.SimilarityPercent;
        Level = pair.Level;
        ReasonCount = pair.Reasons.Count;
    }

    public void LoadIntoCompareVm(CompareViewModel vm)
    {
        if (_filePair != null)
            vm.LoadFilePair(_filePair);
        else if (_subPair != null)
            vm.LoadSubmissionPair(_subPair);
    }
}
