using Avalonia.Controls;
using Avalonia.Input;
using Analogi.App.ViewModels;
using Analogi.App.ViewModels.Pages;

namespace Analogi.App.Views.Pages;

public partial class ResultsPage : UserControl
{
    public ResultsPage()
    {
        InitializeComponent();
    }

    private void DataGrid_DoubleTapped(object? sender, TappedEventArgs e)
    {
        if (sender is DataGrid dg && dg.SelectedItem is ResultItemViewModel item)
        {
            var mainVm = (TopLevel.GetTopLevel(this)?.DataContext) as MainWindowViewModel;
            if (mainVm == null) return;

            item.LoadIntoCompareVm(mainVm.CompareVm);

            // Navigate to Compare tab
            if (TopLevel.GetTopLevel(this) is MainWindow mw)
            {
                mw.NavigateToCompare();
            }
        }
    }
}
