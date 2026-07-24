using System.Collections.ObjectModel;
using Analogi.Core.Models;
using Analogi.Core.Pipeline;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Analogi.App.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ScanCommand))]
    private string _folderPath = string.Empty;

    [ObservableProperty]
    private bool _isScanning;

    [ObservableProperty]
    private string _statusText = "Select a folder to scan.";

    [ObservableProperty]
    private int _progressValue;

    [ObservableProperty]
    private int _progressMax = 100;

    [ObservableProperty]
    private PlagiarismLevel? _filterLevel;

    /// <summary>Set by the View (needs platform StorageProvider access).</summary>
    public IRelayCommand? BrowseCommand { get; set; }

    public ObservableCollection<FilePairResult> Results { get; } = [];
    public ObservableCollection<FilePairResult> FilteredResults { get; } = [];

    private ScanResult? _lastScanResult;

    private bool CanScan() => !string.IsNullOrWhiteSpace(FolderPath) && !IsScanning;

    [RelayCommand(CanExecute = nameof(CanScan))]
    private async Task ScanAsync(CancellationToken ct)
    {
        if (!Directory.Exists(FolderPath))
        {
            StatusText = "Folder does not exist.";
            return;
        }

        IsScanning = true;
        StatusText = "Scanning...";
        ProgressValue = 0;
        Results.Clear();
        FilteredResults.Clear();

        var progress = new Progress<(int current, int total)>(p =>
        {
            ProgressMax = p.total;
            ProgressValue = p.current;
            StatusText = $"Comparing pair {p.current}/{p.total}...";
        });

        try
        {
            var engine = new AnalysisEngine();
            _lastScanResult = await engine.ScanAsync(FolderPath, progress, ct);

            foreach (var pair in _lastScanResult.Pairs.OrderByDescending(p => p.SimilarityIndex))
                Results.Add(pair);

            FilterLevel = null;
            ApplyFilter();

            StatusText = $"Done. {_lastScanResult.TotalFiles} files, {_lastScanResult.Pairs.Count} pairs found in {_lastScanResult.Duration.TotalSeconds:F1}s.";
        }
        catch (OperationCanceledException)
        {
            StatusText = "Scan cancelled.";
        }
        catch (Exception ex)
        {
            StatusText = $"Error: {ex.Message}";
        }
        finally
        {
            IsScanning = false;
        }
    }

    [RelayCommand]
    private void SetFilter(string? level)
    {
        FilterLevel = level switch
        {
            "All" => null,
            "High" => PlagiarismLevel.High,
            "Moderate" => PlagiarismLevel.Moderate,
            "Low" => PlagiarismLevel.Low,
            _ => null
        };
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        FilteredResults.Clear();
        foreach (var r in Results)
        {
            if (FilterLevel == null || (int)r.Level >= (int)FilterLevel)
                FilteredResults.Add(r);
        }
    }
}
