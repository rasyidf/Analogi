using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using Ookii.Dialogs.Wpf;
using Analogi.Core.Extractors;
using Analogi.Framework;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls;
using System.Threading;

namespace Analogi.Core
{       
    internal class ShellViewModel : ViewModel
    {
        #region Properties

        public ICommand BrowseCommand => new DelegateCommand(BrowseFolder);

        public ICollectionView CollectionView { get; private set; }

        public ObservableCollection<DetectionResult> DistanceFiltered
        {
            get
            {
                if (distances == null) return distances;

                IEnumerable<DetectionResult> collection = distances.Where(c =>
                   {
                       switch (FilterMode)
                       {
                           case PlagiarismLevel.Extreme:
                           case PlagiarismLevel.VeryHigh:
                               return (int)c.PlagiarismLevel == (int)PlagiarismLevel.VeryHigh || (int)c.PlagiarismLevel == (int)PlagiarismLevel.Extreme;
                           case PlagiarismLevel.High:
                           case PlagiarismLevel.Moderate:
                               return (int)c.PlagiarismLevel == (int)PlagiarismLevel.Moderate || (int)c.PlagiarismLevel == (int)PlagiarismLevel.High;
                           case PlagiarismLevel.Low:
                           case PlagiarismLevel.Minor:
                               return (int)c.PlagiarismLevel == (int)PlagiarismLevel.Minor || (int)c.PlagiarismLevel == (int)PlagiarismLevel.Low;
                           case PlagiarismLevel.Original:
                           case PlagiarismLevel.None:
                               return (int)c.PlagiarismLevel == (int)PlagiarismLevel.Original;
                           case PlagiarismLevel.All:
                               return c != null;
                           default:
                               return c.Index > 0;
                       }
                   }
                );
                return new ObservableCollection<DetectionResult>(collection);
            }

            set
            {
                RaisePropertyChanged(nameof(DistanceFiltered));
            }
        }

        public ObservableCollection<DetectionResult> Distances { get => distances; set { distances = value; RaisePropertyChanged(nameof(Distances)); } }
        public ICommand FilterOriginalCommand => new DelegateCommand(FilterOriginal);
        public ICommand FilterAnyCommand => new DelegateCommand(FilterAny);
        public ICommand FilterAllCommand => new DelegateCommand(FilterAll);
        public ICommand FilterHighCommand => new DelegateCommand(FilterHigh);
        public ICommand FilterMediumCommand => new DelegateCommand(FilterMedium);
        public ICommand FilterLowCommand => new DelegateCommand(FilterLow);
        public ICommand ResetViewCommand => new DelegateCommand(ResetView);
        public string Path { get => path; set { path = value; RaisePropertyChanged(nameof(Path)); if (!string.IsNullOrEmpty(Path)) { StartVisible = Visibility.Visible; } else { StartVisible = Visibility.Collapsed; } } }
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

        private ObservableCollection<DetectionResult> distances;
        private PlagiarismLevel FilterMode = PlagiarismLevel.Original;
        private string path = @"No Folder Selected";
        private Visibility startVisible = Visibility.Collapsed;
        private Visibility filterVisible = Visibility.Collapsed; 

        #endregion Fields

        #region Methods
        //untuk upload sesuai dengan file dengan ekstensi yang disetujui
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
            FilterMode = PlagiarismLevel.All; RaisePropertyChanged(nameof(DistanceFiltered));
            FilterVisible = Visibility.Visible;
        }

        public void StartTask()
        {
            //Antarmuka, Abaikan
            var a = DialogHost.Show(new TextBlock(){Text = "Please Wait"}, delegate (object sender, DialogOpenedEventArgs args)
            {
                Thread.Sleep(100);
                args.Session.Close(false);
            }
            );

            // Inisialisasi Scanner
            IScanner scanner;
            // pembacaan file yang di upload berupa folder atau sub folder
            if (System.IO.Directory.EnumerateFiles(Path).Count() == 0 && System.IO.Directory.EnumerateDirectories(Path).Count() > 1)
            {
                // Jika path adalah folder dengan banyak Subfolder
                scanner = new Scanner.SubfolderScanner(Path);
            } else
            {
                // Jika path adalah folder dengan banyak file
                scanner = new Scanner.FolderScanner(Path);
            }

            var engine = new MOSSEngine()
            {
                Scanner = scanner,

                // definisikan proses pemisahan metadata
                Extractors = new List<IExtractor>() { 
                            
                            new CodeExtractor(), // Pemisahan Kode
                            new CommentExtractor(), // Pemisahan Komentar  
                            new StructureExtractor(), // Pemisahan Stuktur
                },

                // definisi proses.
                Pipelines = new List<IPipeline>() {   
                            // PREPROSES
                            new PreProcessors.CodeFilter(),             // menghapus spasi ganda
                            new PreProcessors.Lowercaser(),             //  proses case folding
                            
                            // CORE PROCESS
                            // daftar item yang dibandingkan berdasar pada keterangann bab 2
                            new Analyzers.FileLengthAnalyzer(),         // berdasarkan panjang programnya
                            new Analyzers.CommentLineAnalyzer(),        // berdasarkan panjang komentarnya
                            new Analyzers.CodeLineAnalyzer(),           // berdasar jumlah baris kode
                            new Analyzers.CosineSimilarityAnalyzer(),   // beradasarkan cosine similaritynya

                            // TODO : Yang mungkin bisa ditambahkan, belum diimplementasikan

                            new Analyzers.StructureAnalyzer(),       // berdasarkan struktur codingannya
                            // new Analyzers.FunctionCountAnalyzer(),   // berdasarkan jumlah fungsi

                            // POST PROCESS
                            // pembersihan data dari memori ketika selesai melakukan deteksi
                            new PostProcess.Cleanup()
                }
            };

            engine.Start();
            Distances = new ObservableCollection<DetectionResult>(engine.DetectionResults);
            
        }

        

        private void ResetView()
        {
            StartVisible = Visibility.Collapsed;
            FilterVisible = Visibility.Collapsed;
            Path = "";
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