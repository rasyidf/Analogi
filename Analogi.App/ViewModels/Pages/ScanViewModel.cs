using System.Collections.ObjectModel;
using Analogi.Core.Models;
using Analogi.Core.Pipeline;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Analogi.App.ViewModels.Pages;

public partial class ScanViewModel : ViewModelBase
{
    private readonly ResultsViewModel _resultsVm;
    private readonly CompareViewModel _compareVm;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ScanCommand))]
    private string _folderPath = string.Empty;

    [ObservableProperty]
    private bool _isScanning;

    [ObservableProperty]
    private bool _isSubmissionMode;

    [ObservableProperty]
    private string _statusText = "Select a folder containing source files to scan for code clones.";

    [ObservableProperty]
    private int _progressValue;

    [ObservableProperty]
    private int _progressMax = 100;

    /// <summary>Set by the View for platform folder picker access.</summary>
    public IAsyncRelayCommand? BrowseCommand { get; set; }

    public ScanViewModel(ResultsViewModel resultsVm, CompareViewModel compareVm)
    {
        _resultsVm = resultsVm;
        _compareVm = compareVm;
    }

    private bool CanScan() => !string.IsNullOrWhiteSpace(FolderPath) && !IsScanning;

    [RelayCommand(CanExecute = nameof(CanScan))]
    private async Task ScanAsync(CancellationToken ct)
    {
        if (!Directory.Exists(FolderPath))
        {
            StatusText = "⚠ Folder does not exist.";
            return;
        }

        IsScanning = true;
        StatusText = "Scanning...";
        ProgressValue = 0;

        var progress = new Progress<(int current, int total)>(p =>
        {
            ProgressMax = p.total;
            ProgressValue = p.current;
            StatusText = $"Comparing pair {p.current}/{p.total}...";
        });

        try
        {
            var engine = new AnalysisEngine();

            if (IsSubmissionMode)
            {
                var result = await engine.ScanSubmissionsAsync(FolderPath, progress, ct);
                _resultsVm.LoadSubmissionResults(result);
                StatusText = $"✓ Done. {result.TotalSubmissions} submissions, {result.Pairs.Count} pairs in {result.Duration.TotalSeconds:F1}s.";
            }
            else
            {
                var result = await engine.ScanAsync(FolderPath, progress, ct);
                _resultsVm.LoadFileResults(result);
                StatusText = $"✓ Done. {result.TotalFiles} files, {result.Pairs.Count} pairs in {result.Duration.TotalSeconds:F1}s.";
            }
        }
        catch (OperationCanceledException)
        {
            StatusText = "Scan cancelled.";
        }
        catch (Exception ex)
        {
            StatusText = $"⚠ Error: {ex.Message}";
        }
        finally
        {
            IsScanning = false;
        }
    }
}
