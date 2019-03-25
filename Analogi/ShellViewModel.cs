using Ookii.Dialogs.Wpf;
using rasyidf.Analogi.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input; 

namespace Analogi
{
    class ShellViewModel : ViewModelBase
    {
        #region Properties
         
        public ICommand BrowseCommand => new DelegateCommand(BrowseFolder);

    

        public ObservableCollection<DetectionResult> Distances { get => distances; set { distances = value; NotifyProps(nameof(Distances)); } }
        public string Path { get => path; set { path = value; NotifyProps(nameof(Path)); if (!String.IsNullOrEmpty(Path)) { StartVisible = Visibility.Visible; } else { StartVisible = Visibility.Collapsed; } } }
        public Visibility StartVisible { get => startVisible; set { startVisible = value; NotifyProps(nameof(StartVisible)); } }
        public ICommand ScanCommand => new DelegateCommand(ScanFolder); 
        #endregion Properties

        #region Fields 
        private ObservableCollection<DetectionResult> distances;
        string path = @"No Folder Selected";
        private Visibility startVisible = Visibility.Collapsed;

        #endregion Fields

        #region Methods
        private void BrowseFolder()
        {
            var a = new VistaFolderBrowserDialog();
            if (a.ShowDialog() == true)
            {
                Path = a.SelectedPath;
            } 
        }

        public void ScanFolder()
        {
            if (path == "" || path == "No Folder Selected" )
            {
                MessageBox.Show("What should i do, there's no directory to scan");
                return;
            }

            if (!System.IO.Directory.Exists(path) )
            {
                MessageBox.Show("What should i do, you've entered wrong directory, it doesn't exist");
                return;
            }

            StartTask();

        }
        public void StartTask()
        {
            var a = new PlagiarismDetect(Path);
            a.Start();
            Distances = new ObservableCollection<DetectionResult>(a.DetectionResult); 
        }
        #endregion Methods
    }
}
