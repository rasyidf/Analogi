using Avalonia;
using Avalonia.Styling;
using Analogi.App.ViewModels.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Analogi.App.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ScanViewModel ScanVm { get; }
    public ResultsViewModel ResultsVm { get; }
    public CompareViewModel CompareVm { get; }
    public SettingsViewModel SettingsVm { get; }

    public MainWindowViewModel()
    {
        ResultsVm = new ResultsViewModel();
        CompareVm = new CompareViewModel();
        SettingsVm = new SettingsViewModel();
        ScanVm = new ScanViewModel(ResultsVm, CompareVm);
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        var app = Application.Current;
        if (app is null) return;

        app.RequestedThemeVariant = app.ActualThemeVariant == ThemeVariant.Dark
            ? ThemeVariant.Light
            : ThemeVariant.Dark;
    }
}
