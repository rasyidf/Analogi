using Avalonia.Controls;
using Analogi.App.ViewModels.Pages;

namespace Analogi.App.Views.Pages;

public partial class ComparePage : UserControl
{
    public ComparePage()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (DataContext is CompareViewModel vm)
        {
            vm.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(CompareViewModel.FileAContent))
                    EditorA.Text = vm.FileAContent;
                if (args.PropertyName == nameof(CompareViewModel.FileBContent))
                    EditorB.Text = vm.FileBContent;
            };

            // Set initial content if already loaded
            if (!string.IsNullOrEmpty(vm.FileAContent))
                EditorA.Text = vm.FileAContent;
            if (!string.IsNullOrEmpty(vm.FileBContent))
                EditorB.Text = vm.FileBContent;
        }
    }
}
