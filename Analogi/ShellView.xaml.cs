using Ookii.Dialogs.Wpf;
using rasyidf.Analogi.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Analogi
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView 
    { 
        private ShellViewModel ViewModel;
 
        public ShellView()
        {
            InitializeComponent();
            ViewModel = new ShellViewModel();
            this.DataContext = ViewModel;
        } 

        private void SelectedFolderTb_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            StartButton.Visibility = Visibility.Collapsed; 
        }

        private void Button_PreviewDragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true; 
        }

        private void Button_PreviewDrop(object sender, DragEventArgs e)
        {
            object text = e.Data.GetData(DataFormats.FileDrop);

            this.ViewModel.Path = $"{((string[])text)[0]}";
        }

        private void LResult_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowMoreReasons();
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
                    MessageBox.Show(sb.ToString(), "Analogi");
                }

            }
        }

        private void LResult_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                ShowMoreReasons();
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.ViewModel.StartTask();
            }
        }
         
    }
}
