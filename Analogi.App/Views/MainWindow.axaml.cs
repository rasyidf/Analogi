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
        }
    }

    private void NavList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ContentFrame is null) return;
        if (sender is ListBox lb && lb.SelectedItem is ListBoxItem item && item.Tag is string tag)
        {
            ContentFrame.Content = tag switch
            {
                "Scan" => _scanPage,
                "Results" => _resultsPage,
                "Compare" => _comparePage,
                "Settings" => _settingsPage,
                "About" => _aboutPage,
                _ => _scanPage
            };
        }
    }
}
