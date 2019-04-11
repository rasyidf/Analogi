/*
 * MIT
 */

using System;
using System.Collections.Generic;

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

                double r = 1;
                foreach (IReason item in Reasons)
                {
                    if (item.Index < item.Treshold)
                    {
                        continue;
                    }

                    r *= item.Index * item.Bias;
                }
                return r;
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

        #endregion Properties
    }
}