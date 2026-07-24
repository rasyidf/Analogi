using Analogi.App.ViewModels.Pages;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Analogi.App.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ScanViewModel ScanVm { get; }
    public ResultsViewModel ResultsVm { get; }
    public CompareViewModel CompareVm { get; }

    public MainWindowViewModel()
    {
        ResultsVm = new ResultsViewModel();
        CompareVm = new CompareViewModel();
        ScanVm = new ScanViewModel(ResultsVm, CompareVm);
    }
}
