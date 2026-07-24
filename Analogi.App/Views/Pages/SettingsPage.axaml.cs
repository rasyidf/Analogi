using Avalonia.Controls;
using Avalonia.Input;
using Analogi.App.ViewModels.Pages;

namespace Analogi.App.Views.Pages;

public partial class SettingsPage : UserControl
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    private void StepCard_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border border && border.Tag is PipelineStepViewModel vm)
        {
            vm.IsEnabled = !vm.IsEnabled;
        }
    }
}
