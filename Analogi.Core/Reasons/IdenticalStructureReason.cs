using System;
using System.IO;

namespace Analogi.Core
{
    public class IdenticalStructureReason : IReason
    {
        #region Properties

        public double Weight { get; set; } = 0.3;

        public string Region { get; set; }
        public double Index { get; set; }
        public string ReasonString => $"High structural similarity at '{Region}' with: {TargetFile}";

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

            Index = new Cosine().Similarity(source, target);
            return Index;
        }

        #endregion Methods
    }
}