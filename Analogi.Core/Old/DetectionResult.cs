/*
 * MIT
 */

using rasyidf.Analogi.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Media;

namespace rasyidf.Analogi.Core
{
    public class DetectionResult
    {
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

        public double Index
        {
            get
            {
                if (Reasons.Count == 0)
                {
                    return 0;
                }

                double r = 0, p=0;
                for (int i = 0; i < Reasons.Count; i++)
                {
                    IReason item = Reasons[i];
                    if (item.Index < item.Treshold)
                    {
                        continue;
                    }
                    else p++;

                    r += item.Index * item.Bias;
                }
                return r / p;
            }
        }
        public SolidColorBrush IndexColor
        {
            get
            {
                ColorHelper.HsvToRgb(100 - IndexPercentage, 1, 1, out int r, out int g, out int b);
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