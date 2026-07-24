using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Analogi.App.ViewModels.Pages;
using CommunityToolkit.Mvvm.Input;

namespace Analogi.App.Views.Pages;

public partial class ScanPage : UserControl
{
    public ScanPage()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (DataContext is ScanViewModel vm)
        {
            vm.BrowseCommand = new AsyncRelayCommand(BrowseFolderAsync);
        }
    }

    private async Task BrowseFolderAsync()
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select folder to scan",
            AllowMultiple = false
        });

        if (folders.Count > 0 && DataContext is ScanViewModel vm)
        {
            vm.FolderPath = folders[0].Path.LocalPath;
        }
    }
}
