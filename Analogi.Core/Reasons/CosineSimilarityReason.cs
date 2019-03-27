
using System.IO;

namespace rasyidf.Analogi.Core
{
    public class CosineSimilarityReason : IReason
    {
        public string ReasonString => $"High similarity index with: {TargetFile}";

        public string TargetFile { get; private set; }

        public double Index { get; set; }
        public double Bias { get; set; } = 1;
        public double Treshold { get => 0.8f; }

        public void SetTargetFile(string value)
        {
            TargetFile = new FileInfo(value).Name;
        }

        public double Check(ref string source, ref string target)
        {
            Index = new Cosine().Similarity(source, target);
            return Index;
        }
    }
}
