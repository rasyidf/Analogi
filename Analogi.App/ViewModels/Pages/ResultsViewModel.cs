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

    public ObservableCollection<FilePairResult> FilePairResults { get; } = [];
    public ObservableCollection<SubmissionPairResult> SubmissionPairResults { get; } = [];
    public ObservableCollection<object> FilteredResults { get; } = [];

    [ObservableProperty]
    private bool _isFileMode = true;

    private ScanResult? _scanResult;
    private SubmissionScanResult? _submissionResult;
    private PlagiarismLevel? _filterLevel;

    public void LoadFileResults(ScanResult result)
    {
        _scanResult = result;
        _submissionResult = null;
        IsFileMode = true;

        FilePairResults.Clear();
        foreach (var pair in result.Pairs.OrderByDescending(p => p.SimilarityIndex))
            FilePairResults.Add(pair);

        _filterLevel = null;
        ApplyFilter();
        SummaryText = $"{result.TotalFiles} files scanned, {result.Pairs.Count} similar pairs found.";
    }

    public void LoadSubmissionResults(SubmissionScanResult result)
    {
        _submissionResult = result;
        _scanResult = null;
        IsFileMode = false;

        SubmissionPairResults.Clear();
        foreach (var pair in result.Pairs.OrderByDescending(p => p.SimilarityIndex))
            SubmissionPairResults.Add(pair);

        _filterLevel = null;
        ApplyFilter();
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
        if (IsFileMode)
        {
            foreach (var r in FilePairResults)
            {
                if (_filterLevel == null || (int)r.Level >= (int)_filterLevel)
                    FilteredResults.Add(r);
            }
        }
        else
        {
            foreach (var r in SubmissionPairResults)
            {
                if (_filterLevel == null || (int)r.Level >= (int)_filterLevel)
                    FilteredResults.Add(r);
            }
        }
    }
}
