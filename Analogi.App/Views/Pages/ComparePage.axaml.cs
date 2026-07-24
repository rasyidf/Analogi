using Avalonia.Controls;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using TextMateSharp.Grammars;
using Analogi.App.ViewModels.Pages;

namespace Analogi.App.Views.Pages;

public partial class ComparePage : UserControl
{
    private bool _textMateInitialized;

    public ComparePage()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (DataContext is CompareViewModel vm)
        {
            InitTextMate();

            vm.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(CompareViewModel.FileAContent))
                    EditorA.Text = vm.FileAContent;
                if (args.PropertyName == nameof(CompareViewModel.FileBContent))
                    EditorB.Text = vm.FileBContent;
            };

            if (!string.IsNullOrEmpty(vm.FileAContent))
                EditorA.Text = vm.FileAContent;
            if (!string.IsNullOrEmpty(vm.FileBContent))
                EditorB.Text = vm.FileBContent;
        }
    }

    private void InitTextMate()
    {
        if (_textMateInitialized) return;
        _textMateInitialized = true;

        var registryOptions = new RegistryOptions(ThemeName.DarkPlus);

        var installationA = EditorA.InstallTextMate(registryOptions);
        installationA.SetGrammar(registryOptions.GetScopeByLanguageId("cpp"));

        var installationB = EditorB.InstallTextMate(registryOptions);
        installationB.SetGrammar(registryOptions.GetScopeByLanguageId("cpp"));
    }
}
