﻿using System.IO;

namespace rasyidf.Analogi.Core
{
    public class CosineSimilarityReason : IReason
    {
        #region Properties

        public double Bias { get; set; } = 1;
        public double Index { get; set; }
        public string ReasonString => $"High similarity index with: {TargetFile}";

        public string TargetFile { get; private set; }
        public double Treshold => 0.6f;

        #endregion Properties

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