using Avalonia.Controls;
using Analogi.App.ViewModels;
using Analogi.App.ViewModels.Pages;
using Analogi.Core.Models;

namespace Analogi.App.Views.Pages;

public partial class ResultsPage : UserControl
{
    public ResultsPage()
    {
        InitializeComponent();
    }

    private void DataGrid_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is DataGrid dg && dg.SelectedItem != null)
        {
            // Navigate to compare view with selected pair
            var mainVm = FindMainViewModel();
            if (mainVm == null) return;

            if (dg.SelectedItem is FilePairResult filePair)
            {
                mainVm.CompareVm.LoadFilePair(filePair);
            }
            else if (dg.SelectedItem is SubmissionPairResult subPair)
            {
                mainVm.CompareVm.LoadSubmissionPair(subPair);
            }
        }
    }

    private MainWindowViewModel? FindMainViewModel()
    {
        var topLevel = TopLevel.GetTopLevel(this);
        return topLevel?.DataContext as MainWindowViewModel;
    }
}
