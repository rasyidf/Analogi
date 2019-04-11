using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using Ookii.Dialogs.Wpf;
using rasyidf.Analogi.Core.Extractors;

namespace rasyidf.Analogi.Core
{
    public enum FilterModes
    {
        Original,
        Plagiarism,
        Any
    }

    internal class ShellViewModel : ViewModelBase
    {
        #region Properties

        public ICommand BrowseCommand => new DelegateCommand(BrowseFolder);

        public ICollectionView CollectionView { get; private set; }

        public ObservableCollection<DetectionResult> DistanceFiltered
        {
            get
            {
                switch (FilterMode)
                {
                    case FilterModes.Original:
                        return new ObservableCollection<DetectionResult>(distances.Where(c => c.Index <= 0.5f));

                    case FilterModes.Plagiarism:
                        return new ObservableCollection<DetectionResult>(distances.Where(c => c.Index > 0.5f));

                    default:
                        return Distances;
                }
            }

            set
            {
                distances = value; NotifyProps(nameof(DistanceFiltered));
            }
        }

        public ObservableCollection<DetectionResult> Distances { get => distances; set { distances = value; NotifyProps(nameof(Distances)); } }
        public ICommand FilterOriginalCommand => new DelegateCommand(FilterOriginal);
        public ICommand FilterPlagiatorCommand => new DelegateCommand(FilterPlagiator);
        public string Path { get => path; set { path = value; NotifyProps(nameof(Path)); if (!string.IsNullOrEmpty(Path)) { StartVisible = Visibility.Visible; } else { StartVisible = Visibility.Collapsed; } } }
        public ICommand ScanCommand => new DelegateCommand(ScanFolder);
        public Visibility StartVisible { get => startVisible; set { startVisible = value; NotifyProps(nameof(StartVisible)); } }

        private void FilterOriginal()
        {
            FilterMode = FilterModes.Original; NotifyProps(nameof(DistanceFiltered));
        }

        private void FilterPlagiator()
        {
            FilterMode = FilterModes.Plagiarism; NotifyProps(nameof(DistanceFiltered));
        }

        #endregion Properties

        #region Fields

        private ObservableCollection<DetectionResult> distances;
        private FilterModes FilterMode = FilterModes.Any;
        private string path = @"No Folder Selected";
        private Visibility startVisible = Visibility.Collapsed;

        #endregion Fields

        #region Methods

        public void ScanFolder()
        {
            if (path == "" || path == "No Folder Selected")
            {
                MessageBox.Show("What should i do, there's no directory to scan");
                return;
            }

            if (!System.IO.Directory.Exists(path))
            {
                MessageBox.Show("What should i do, you've entered wrong directory, it doesn't exist");
                return;
            }

            StartTask();
            FilterMode = FilterModes.Any; NotifyProps(nameof(DistanceFiltered));
        }

        public void StartTask()
        {
            
            var engine = new MOSSEngine(new Scanner.FolderScanner(Path))
            {
                Extractors = new List<IExtractor>() {    
                            new CodeExtractor(),
                            new CommentExtractor(),
                },

                Pipelines = new List<IPipeline>() {     
                            new PreProcessors.CodeFilter(), 
                            new PreProcessors.Lowercaser(),      
                            
                            new Analyzers.FileLengthAnalyzer(),
                            new Analyzers.CommentLineAnalyzer(),
                            new Analyzers.CodeLineAnalyzer(),
                            new Analyzers.CosineSimilarityAnalyzer(),
                            new Analyzers.StructureAnalyzer(),     

                            new PostProcess.Cleanup()
                }
            };

            engine.Start();
            Distances = new ObservableCollection<DetectionResult>(engine.DetectionResults);
                                                                    
        }



        private void BrowseFolder()
        {
            var a = new VistaFolderBrowserDialog();
            if (a.ShowDialog() == true)
            {
                Path = a.SelectedPath;
            }
        }

        #endregion Methods
    }
         
}