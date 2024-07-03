using Analogi.Core.Algorithm;
using Analogi.Core.Interfaces;
using System;
using System.IO;

namespace Analogi.Core.Reasons
{
    public class CosineSimilarityReason : IReason
    {
        #region Properties

        public double Weight { get; set; } = 1;
        public double Index { get; set; }
        public string ReasonString => $"High similarity index with: {TargetFile}";

        public string TargetFile { get; private set; }
        public double Treshold => 0.67f;

        #endregion Properties

        public int IndexPercentage => Convert.ToInt32(Index * 100);
        #region Methods


        public void SetTargetFile(string value)
        {
            TargetFile = new FileInfo(value).Name;
        }

        /// <summary>
        /// Kalkulasi Cosine Similarity
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public double Check(string source, string target)
        {

            Index = string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target) ? 0 : new Cosine().Similarity(source, target);
            return Index;
        }

        #endregion Methods
    }
}