using Analogi.Core;
using Analogi.Core.Extractors;
using Analogi.Core.Interfaces;
using Analogi.Framework;
using MaterialDesignThemes.Wpf;
using Ookii.Dialogs.Wpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Analogi.Views
{
    internal class ShellViewModel : ViewModel
    {
        private readonly BackgroundWorker worker = new();

        #region Properties

        public ICommand BrowseCommand => new DelegateCommand(BrowseFolder);

        public ICollectionView CollectionView { get; private set; }

        public ObservableCollection<DetectionResultViewModel> DistanceFiltered
        {
            get
            {
                if (distances == null)
                {
                    return distances;
                }

                IEnumerable<DetectionResultViewModel> collection = distances.Where(c =>
                   {
                       return FilterMode switch
                       {
                           PlagiarismLevel.Extreme or PlagiarismLevel.VeryHigh => (int)c.Model.PlagiarismLevel is ((int)PlagiarismLevel.VeryHigh) or ((int)PlagiarismLevel.Extreme),
                           PlagiarismLevel.High or PlagiarismLevel.Moderate => (int)c.Model.PlagiarismLevel is ((int)PlagiarismLevel.Moderate) or ((int)PlagiarismLevel.High),
                           PlagiarismLevel.Low or PlagiarismLevel.Minor => (int)c.Model.PlagiarismLevel is ((int)PlagiarismLevel.Minor) or ((int)PlagiarismLevel.Low),
                           PlagiarismLevel.Original or PlagiarismLevel.None => c.Model.PlagiarismLevel == (int)PlagiarismLevel.Original,
                           PlagiarismLevel.All => c != null,
                           _ => c.Model.Index > 0,
                       };
                   }
                );
                return new ObservableCollection<DetectionResultViewModel>(collection);
            }

            set => RaisePropertyChanged(nameof(DistanceFiltered));
        }
        public ObservableCollection<DetectionResultViewModel> Distances { get => distances; set { distances = value; RaisePropertyChanged(nameof(Distances)); } }
        public ICommand FilterOriginalCommand => new DelegateCommand(FilterOriginal);
        public ICommand FilterAnyCommand => new DelegateCommand(FilterAny);
        public ICommand FilterAllCommand => new DelegateCommand(FilterAll);
        public ICommand FilterHighCommand => new DelegateCommand(FilterHigh);
        public ICommand FilterMediumCommand => new DelegateCommand(FilterMedium);
        public ICommand FilterLowCommand => new DelegateCommand(FilterLow);
        public ICommand ResetViewCommand => new DelegateCommand(ResetView);
        public string Path { get => path; set { path = value; RaisePropertyChanged(nameof(Path)); StartVisible = !string.IsNullOrEmpty(Path) ? Visibility.Visible : Visibility.Collapsed; } }
        public ICommand ScanCommand => new DelegateCommand(ScanFolder);
        public Visibility StartVisible { get => startVisible; set { startVisible = value; RaisePropertyChanged(nameof(StartVisible)); } }
        public Visibility FilterVisible { get => filterVisible; set { filterVisible = value; RaisePropertyChanged(nameof(FilterVisible)); } }

        private void FilterOriginal()
        {
            FilterMode = PlagiarismLevel.Original; RaisePropertyChanged(nameof(DistanceFiltered));
        }
        private void FilterAny()
        {
            FilterMode = PlagiarismLevel.None; RaisePropertyChanged(nameof(DistanceFiltered));
        }

        private void FilterAll()
        {
            FilterMode = PlagiarismLevel.All; RaisePropertyChanged(nameof(DistanceFiltered));
        }
        private void FilterHigh()
        {
            FilterMode = PlagiarismLevel.VeryHigh;
            RaisePropertyChanged(nameof(DistanceFiltered));
        }
        private void FilterMedium()
        {
            FilterMode = PlagiarismLevel.Moderate; RaisePropertyChanged(nameof(DistanceFiltered));
        }
        private void FilterLow()
        {
            FilterMode = PlagiarismLevel.Low; RaisePropertyChanged(nameof(DistanceFiltered));
        }

        #endregion Properties

        #region Fields

        private ObservableCollection<DetectionResultViewModel> distances;
        private PlagiarismLevel FilterMode = PlagiarismLevel.Original;
        private string path = @"No Folder Selected";
        private Visibility startVisible = Visibility.Collapsed;
        private Visibility filterVisible = Visibility.Collapsed;
        private MOSSEngine engine;

        #endregion Fields

        #region Methods 
        // upload based on file with approved extension
        public void ScanFolder()
        {
            if (path is "" or "No Folder Selected")
            {
                _ = MessageBox.Show("What should i do, there's no directory to scan");
                return;
            }

            if (!System.IO.Directory.Exists(path))
            {
                _ = MessageBox.Show("What should i do, you've entered wrong directory, it doesn't exist");
                return;
            }

            InitiateScanProcess();

        }

        public void InitiateScanProcess()
        {
            if (!worker.IsBusy)
            {
                StackPanel content = new()
                {
                    MinHeight = 100,
                    MinWidth = 100,
                    Orientation = Orientation.Vertical,
                    Children = {
                                 new TextBlock() { HorizontalAlignment=HorizontalAlignment.Center, Text = "Calculating", Margin= new Thickness(10) },
                                 new ProgressBar() { Width=50,Height=50, Style= Application.Current.FindResource("MaterialDesignCircularProgressBar") as Style,
                                                 IsIndeterminate=true }
                                 }
                };
                _ = DialogHost.Show(content, dialogIdentifier: "MainDH");


                worker.DoWork += Worker_DoWork;
                worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                engine = InitEngine();
                worker.RunWorkerAsync();
            }
        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            engine.Start();
            e.Result = engine;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
            if (e.Cancelled)
            {
                Debug.WriteLine("Operation Cancelled");
            }
            else if (e.Error != null)
            {
                Debug.WriteLine("Error in Process :" + e.Error);
            }
            else
            {
                Debug.WriteLine("Operation Completed :" + e.Result);

                DialogHost.CloseDialogCommand.Execute(null, null);
                Distances = new ObservableCollection<DetectionResultViewModel>(engine.DetectionResults);
                FilterMode = PlagiarismLevel.All; RaisePropertyChanged(nameof(DistanceFiltered));
                FilterVisible = Visibility.Visible;
            }
        }


        private MOSSEngine InitEngine()
        {

            // Initiate Scanner
            IScanner scanner;
            // file scanning that is uploaded in the form of a folder or subfolder
            if (!System.IO.Directory.EnumerateFiles(Path).Any() && System.IO.Directory.EnumerateDirectories(Path).Any())
            {
                // if the path is a folder with subfolders
                scanner = new Core.Scanner.SubfolderScanner(Path);
            }
            else
            {
                // if the path is a folder with files
                scanner = new Core.Scanner.FolderScanner(Path);
            }

            MOSSEngine engine = new()
            {
                Scanner = scanner,

                // define the extractors
                Extractors = [
                    new CodeExtractor(),
                    new CommentExtractor(),
                    new StructureExtractor(),
                ],

                // define the pipelines
                Pipelines = [   
                            // Preprocessors
                            new Core.PreProcessors.CodeFilter(),
                            new Core.PreProcessors.CaseFold(),            
                            
                            // CORE PROCESS
                            new Core.Analyzers.FileLengthAnalyzer(),
                            new Core.Analyzers.CommentLineAnalyzer(),
                            new Core.Analyzers.CodeLineAnalyzer(),
                            new Core.Analyzers.CosineSimilarityAnalyzer(),

                            new Core.Analyzers.StructureAnalyzer(),     
                            // new Analyzers.FunctionCountAnalyzer(),

                            new Core.PostProcess.Cleanup()
                ]
            };
            return engine;
        }

        private void ResetView()
        {
            StartVisible = Visibility.Collapsed;
            FilterVisible = Visibility.Collapsed;
            Path = "";
        }


        private void BrowseFolder()
        {
            VistaFolderBrowserDialog a = new();
            if (a.ShowDialog() == true)
            {
                Path = a.SelectedPath;
            }
        }

        #endregion Methods
    }

}