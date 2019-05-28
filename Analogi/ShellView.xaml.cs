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

        private ShellViewModel ViewModel;

        #endregion Fields

        #region Constructors

        public ShellView()
        {
            InitializeComponent();
            ViewModel1 = new ShellViewModel();
            DataContext = ViewModel1;
        }

        internal ShellViewModel ViewModel1 { get => ViewModel; set => ViewModel = value; }

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

            ViewModel1.Path = $"{((string[])text)[0]}";
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
                    var sb = new StringBuilder();
                    for (int i = 0; i < d.Reasons.Count; i++)
                    {
                        sb.AppendLine(d.Reasons[i].ReasonString);
                    }
                    // MessageBox.Show(sb.ToString(), "Analogi");
                    var dro = new DResultView(d)
                    {
                        Height = this.Height - 40
                    };
                    DialogHost.Show(dro);
                }
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewModel1.StartTask();
            }
        }

        #endregion Methods
    }
}