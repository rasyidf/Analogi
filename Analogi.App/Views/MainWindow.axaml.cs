using Avalonia.Controls;
using Analogi.App.ViewModels;
using Analogi.App.Views.Pages;

namespace Analogi.App.Views;

public partial class MainWindow : Window
{
    private readonly ScanPage _scanPage = new();
    private readonly ResultsPage _resultsPage = new();
    private readonly ComparePage _comparePage = new();
    private readonly SettingsPage _settingsPage = new();
    private readonly AboutPage _aboutPage = new();
    private bool _handlingSelection;

    public MainWindow()
    {
        InitializeComponent();
        ContentFrame.Content = _scanPage;
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (DataContext is MainWindowViewModel vm)
        {
            _scanPage.DataContext = vm.ScanVm;
            _resultsPage.DataContext = vm.ResultsVm;
            _comparePage.DataContext = vm.CompareVm;
            _settingsPage.DataContext = vm.SettingsVm;

            // Auto-navigate to results after scan completes
            vm.ScanVm.OnScanCompleted = () => NavigateToResults();
        }
    }

    public void NavigateToResults()
    {
        _handlingSelection = true;
        NavList.SelectedIndex = 1; // Results
        NavListBottom.SelectedIndex = -1;
        ContentFrame.Content = _resultsPage;
        _handlingSelection = false;
    }

    public void NavigateToCompare()
    {
        _handlingSelection = true;
        NavList.SelectedIndex = 2; // Compare
        NavListBottom.SelectedIndex = -1;
        ContentFrame.Content = _comparePage;
        _handlingSelection = false;
    }

    private void NavList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ContentFrame is null || _handlingSelection) return;
        if (sender is ListBox lb && lb.SelectedItem is ListBoxItem item && item.Tag is string tag)
        {
            _handlingSelection = true;
            if (sender == NavList) NavListBottom.SelectedIndex = -1;
            else NavList.SelectedIndex = -1;

            ContentFrame.Content = tag switch
            {
                "Scan" => _scanPage,
                "Results" => _resultsPage,
                "Compare" => _comparePage,
                "Settings" => _settingsPage,
                "About" => _aboutPage,
                _ => _scanPage
            };
            _handlingSelection = false;
        }
    }
}
