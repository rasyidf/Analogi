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
    public enum FilterModes
    {
        Original, 
        Plagiarism,
        Any
    }
    class ShellViewModel : ViewModelBase
    {
        #region Properties
         
        public ICommand BrowseCommand => new DelegateCommand(BrowseFolder);

        public ICollectionView CollectionView { get; private set; }

        public ObservableCollection<DetectionResult> Distances { get => distances; set { distances = value; NotifyProps(nameof(Distances)); } }

        public ObservableCollection<DetectionResult> DistanceFiltered
        {
            get
            {
                switch (FilterMode)
                {
                    case FilterModes.Original:
                        return new ObservableCollection<DetectionResult>(distances.Where(c => c.Index == 0));
                    case FilterModes.Plagiarism:
                        return new ObservableCollection<DetectionResult>(distances.Where(c => c.Index == 1));
                    default:
                        return Distances;
                }

            }

            set
            {
                distances = value; NotifyProps(nameof(DistanceFiltered));
            }
        }

        public string Path { get => path; set { path = value; NotifyProps(nameof(Path)); if (!String.IsNullOrEmpty(Path)) { StartVisible = Visibility.Visible; } else { StartVisible = Visibility.Collapsed; } } }
        public Visibility StartVisible { get => startVisible; set { startVisible = value; NotifyProps(nameof(StartVisible)); } }
        public ICommand ScanCommand => new DelegateCommand(ScanFolder);
        public ICommand FilterPlagiatorCommand => new DelegateCommand(FilterPlagiator);

        private void FilterPlagiator()
        {
            FilterMode = FilterModes.Plagiarism; NotifyProps(nameof(DistanceFiltered));
        }

        public ICommand FilterOriginalCommand => new DelegateCommand(FilterOriginal);

        private void FilterOriginal()
        {
            FilterMode = FilterModes.Original; NotifyProps(nameof(DistanceFiltered));
        }
        #endregion Properties

        #region Fields 
        private ObservableCollection<DetectionResult> distances;
        string path = @"No Folder Selected";
        private Visibility startVisible = Visibility.Collapsed;
        private FilterModes FilterMode = FilterModes.Any;

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
            FilterMode = FilterModes.Any; NotifyProps(nameof(DistanceFiltered));
        }
        public void StartTask()
        {
            var a = new PlagiarismDetect(Path);
            a.Start();
            Distances = new ObservableCollection<DetectionResult>(a.DetectionResults); 
        }
        #endregion Methods
    }
}
