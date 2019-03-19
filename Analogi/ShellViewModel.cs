using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Yaudah.Core;

namespace Analogi
{
    class ShellViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<string> Algorithms { get => algorithms; set { algorithms = value; NotifyProps(nameof(Algorithms)); } }
        public ICommand CalculateCommand => new DelegateCommand(CalculateDistance);
        public ObservableCollection<DetectionResult> Distances { get => distances; set { distances = value; NotifyProps(nameof(Distances)); } }
        public string Path { get => path; set { path = value; NotifyProps(nameof(Path)); } }
        public ICommand ScanCommand => new DelegateCommand(ScanFolder);
        public ObservableCollection<CodeFile> Scripts { get => scripts; set { scripts = value; NotifyProps(nameof(Scripts)); } }
        public string SelectedAlgorithm { get => selectedAlgorithm; set { selectedAlgorithm = value; NotifyProps(nameof(SelectedAlgorithm)); } }

        public CodeFile Script { get => script; set { script = value;  NotifyProps(nameof(Script)); } }

        #endregion Properties

        #region Fields

        ObservableCollection<String> algorithms = new ObservableCollection<string>()
        { "Cosine", "Levenshtein", "Jaccard", "Damerau", "Jaro Winkler", "LCS" };
        private ObservableCollection<DetectionResult> distances;
        string path = @"D:\DEV\Code\py_3";
        ObservableCollection<CodeFile> scripts = new ObservableCollection<CodeFile>();
        CodeFile script = null;
        string selectedAlgorithm = "Cosine";

        #endregion Fields

        #region Methods

        private void CalculateDistance()
        {
            if (script == null)
            {
                MessageBox.Show("Please select one name");
                return;
            }

            CalculateDistanceWith(script, Scripts);
        }

        private void CalculateDistanceWith(CodeFile script, ObservableCollection<CodeFile> scripts)
        {
            Distances = new ObservableCollection<DetectionResult>();
            foreach (var item in Scripts)
            {
                if (item == script) continue; 

                var Dist = new DetectionResult(script);
                Distances.Add(Dist);
            }

        }

        private void ScanFolder()
        {
            if (path == "" )
            {
                MessageBox.Show("What should i do, there's no directory to scan");
                return;
            }

            if (!System.IO.Directory.Exists(path) )
            {
                MessageBox.Show("What should i do, you've entered wrong directory, it doesn't exist");
                return;
            }
             
            var p = new PlagiarismDetect(path);
        }

        #endregion Methods
    }
}
