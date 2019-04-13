using System;
using System.IO;

namespace Analogi.Core
{
    public class CosineSimilarityReason : IReason
    {
        #region Properties

        public double Bias { get; set; } = 1;
        public double Index { get; set; }
        public string ReasonString => $"High similarity index with: {TargetFile}";

        public string TargetFile { get; private set; }
        public double Treshold => 0.7f;

        #endregion Properties

        public int IndexPercentage => Convert.ToInt32(Index * 100);
        #region Methods


        public void SetTargetFile(string value)
        {
            TargetFile = new FileInfo(value).Name;
        }
                  

        public double Check(string source, string target)
        {
            Index = new Cosine().Similarity(source, target);
            return Index;
        }

        #endregion Methods
    }
}