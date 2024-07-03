using AnalogiUI.Models;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace AnalogiUI.ViewModels.Pages
{
    public partial class DataViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private IEnumerable<DataColor>? _colors;

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
            {
                InitializeViewModel();
            }
        }

        public void OnNavigatedFrom() { }

        private void InitializeViewModel()
        {
            Random random = new();
            List<DataColor> colorCollection = [];

            for (int i = 0; i < 8192; i++)
            {
                colorCollection.Add(
                    new DataColor
                    {
                        Color = new SolidColorBrush(
                            Color.FromArgb(
                                200,
                                (byte)random.Next(0, 250),
                                (byte)random.Next(0, 250),
                                (byte)random.Next(0, 250)
                            )
                        )
                    }
                );
            }

            Colors = colorCollection;

            _isInitialized = true;
        }
    }
}
