using Avalonia.Controls;
using Analogi.App.ViewModels;
using Analogi.App.Views.Pages;
using FluentAvalonia.UI.Controls;

namespace Analogi.App.Views;

public partial class MainWindow : Window
{
    private readonly ScanPage _scanPage = new();
    private readonly ResultsPage _resultsPage = new();
    private readonly ComparePage _comparePage = new();
    private readonly AboutPage _aboutPage = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (DataContext is MainWindowViewModel vm)
        {
            _scanPage.DataContext = vm.ScanVm;
            _resultsPage.DataContext = vm.ResultsVm;
            _comparePage.DataContext = vm.CompareVm;
        }

        // Navigate to scan page by default
        ContentFrame.Content = _scanPage;
    }

    private void NavView_SelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        if (e.SelectedItem is NavigationViewItem item && item.Tag is string tag)
        {
            ContentFrame.Content = tag switch
            {
                "ScanPage" => _scanPage,
                "ResultsPage" => _resultsPage,
                "ComparePage" => _comparePage,
                "AboutPage" => _aboutPage,
                _ => _scanPage
            };
        }
    }
}
