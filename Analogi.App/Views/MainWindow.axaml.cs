using Avalonia.Controls;
using Avalonia.Controls.Primitives;
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

    private void NavTabs_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ContentFrame is null) return;
        if (sender is TabStrip tabs)
        {
            ContentFrame.Content = tabs.SelectedIndex switch
            {
                0 => _scanPage,
                1 => _resultsPage,
                2 => _comparePage,
                3 => _settingsPage,
                4 => _aboutPage,
                _ => _scanPage
            };
        }
    }
}
