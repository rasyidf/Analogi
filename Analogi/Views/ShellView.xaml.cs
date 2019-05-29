using System.Text;
using System.Windows;
using System.Windows.Input;
using Analogi;
using Analogi.Core;
using MaterialDesignThemes.Wpf;

namespace Analogi.Core
{
    /// <summary>Interaction logic for ShellView.xaml</summary>
    public partial class ShellView
    {
        #region Fields


        #endregion Fields

        #region Constructors

        public ShellView()
        {
            InitializeComponent();
            ViewModel = new ShellViewModel();
            DataContext = ViewModel;
        }

        internal ShellViewModel ViewModel { get; set; }

        #endregion Constructors

        #region Methods

        private void Button_PreviewDragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private void Button_PreviewDrop(object sender, DragEventArgs e)
        {
            object text = e.Data.GetData(DataFormats.FileDrop);

            ViewModel.Path = $"{((string[])text)[0]}";
        }

        private void LResult_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                ShowMoreReasons();
            }
        }

        private void LResult_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowMoreReasons();
        }

        private void SelectedFolderTb_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            StartButton.Visibility = Visibility.Collapsed;
        }

        private void ShowMoreReasons()
        {
            if (lResult.SelectedItem != null)
            {
                if (lResult.SelectedItem is DetectionResult d)
                {                                                   
                    var dro = new DResultView(d)
                    {
                        Height = this.Height - 40
                    };
                    DialogHost.Show(dro, dialogIdentifier: "MainDH");
                }
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewModel.StartTask();
            }
        }

        #endregion Methods
    }
}