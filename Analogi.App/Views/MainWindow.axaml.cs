using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Analogi.App.ViewModels;
using CommunityToolkit.Mvvm.Input;

namespace Analogi.App.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (DataContext is MainWindowViewModel vm)
        {
            vm.BrowseCommand = new AsyncRelayCommand(BrowseFolderAsync);
        }
    }

    private async Task BrowseFolderAsync()
    {
        var folders = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select folder to scan",
            AllowMultiple = false
        });

        if (folders.Count > 0 && DataContext is MainWindowViewModel vm)
        {
            vm.FolderPath = folders[0].Path.LocalPath;
        }
    }
}
