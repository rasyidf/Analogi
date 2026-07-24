using System.Collections.ObjectModel;
using Analogi.Core.Interfaces;
using Analogi.Core.Pipeline;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Analogi.App.ViewModels.Pages;

public partial class SettingsViewModel : ViewModelBase
{
    public ObservableCollection<PipelineStepViewModel> Steps { get; } = [];

    public SettingsViewModel()
    {
        var engine = new AnalysisEngine();
        foreach (var step in engine.Steps)
        {
            Steps.Add(new PipelineStepViewModel(step));
        }
    }
}

/// <summary>
/// Wraps an IPipelineStep for UI binding with toggle support.
/// </summary>
public partial class PipelineStepViewModel : ObservableObject
{
    private readonly IPipelineStep _step;

    public string Name => _step.Name;

    public string Category => _step switch
    {
        Analogi.Core.Extractors.CodeExtractor => "Extractor",
        Analogi.Core.Extractors.CommentExtractor => "Extractor",
        Analogi.Core.Extractors.StructureExtractor => "Extractor",
        Analogi.Core.PreProcessors.CaseFold => "PreProcessor",
        Analogi.Core.PreProcessors.WhitespaceNormalize => "PreProcessor",
        Analogi.Core.PreProcessors.StringLiteralNormalize => "PreProcessor",
        Analogi.Core.PreProcessors.IdentifierNormalize => "PreProcessor",
        _ => "Analyzer"
    };

    public string CategoryColor => Category switch
    {
        "Extractor" => "#4FC3F7",
        "PreProcessor" => "#FFB74D",
        "Analyzer" => "#81C784",
        _ => "#90A4AE"
    };

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Opacity))]
    private bool _isEnabled;

    public double Opacity => IsEnabled ? 1.0 : 0.4;

    public PipelineStepViewModel(IPipelineStep step)
    {
        _step = step;
        _isEnabled = step.IsEnabled;
    }

    partial void OnIsEnabledChanged(bool value)
    {
        _step.IsEnabled = value;
    }
}
