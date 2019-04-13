/*
 * MIT
 */

using Analogi.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media;

namespace Analogi.Core
{
    public enum PlagiarismLevel
    {
        Extreme = 100,
        VeryHigh = 90,
        High = 80,
        Moderate = 70,
        Low = 60,
        Minor = 30,
        Original = 0,
        None = -1,
        All = 101
    }
    public class DetectionResult : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Methods

        public void RaisePropertyChanged(string args)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(args));
        }

        #endregion Methods

        #region Fields

        private readonly CodeFile CodeFile;

        #endregion Fields

        #region Constructors

        public DetectionResult(string path)
        {
            CodeFile = new CodeFile(path); Reasons = new List<IReason>();
        }

        public DetectionResult(CodeFile code)
        {
            CodeFile = code; Reasons = new List<IReason>();
        }

        #endregion Constructors

        #region Properties

        public long Length
        {
            get => CodeFile.Length;
        }

        public PlagiarismLevel PlagiarismLevel { get {     
                return (PlagiarismLevel)(Convert.ToInt16(IndexPercentage/10) * 10);
            } }

        public double Index
        {
            get
            {
                if (Reasons.Count == 0)
                {
                    return 0;
                }

                double r = 0, p=0, max=0;
                for (int i = 0; i < Reasons.Count; i++)
                {
                    IReason item = Reasons[i];
                    if (item.Index < item.Treshold)
                    {
                        continue;
                    }
                    else p++;

                    r += item.Index * item.Bias;
                    max = Math.Max(Math.Max(r/p, max), item.Index * item.Bias);
                }
                // return r / p;
                return max;
            }
        }
        public SolidColorBrush IndexColor
        {
            get
            {
                ColorHelper.HsvToRgb(80 - 80 * Index,
                                     1,
                                     1,
                                     out int r,
                                     out int g,
                                     out int b);
                return new SolidColorBrush(Color.FromRgb((byte)r, (byte)g, (byte)b));
            }   

        }
     
        public int IndexPercentage => Convert.ToInt32(Index * 100);
        public string Name => CodeFile.Name;

        public string Reason
        {
            get
            {
                if (Reasons.Count > 1)
                {
                    return "There are more than one reason, double click to see details";
                }
                else if (Reasons.Count == 1)
                {
                    return Reasons[0].ReasonString;
                }
                return "File is Fine";
            }
        }

        public List<IReason> Reasons { get; set; }
        public ObservableCollection<IReason> ReasonsList
        {
            get
            {
                return new ObservableCollection<IReason>(Reasons);
            }
        }

        #endregion Properties
    }
}