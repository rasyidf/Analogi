
using System.IO;

namespace rasyidf.Analogi.Core
{
    public class CosineSimilarityReason : IReason
    {
        public string ReasonString => $"High similarity index with: {TargetFile}";

        public string TargetFile { get; private set; }

        public double Index { get; set; }
        public double Bias { get; set; } = 1;

        public void SetTargetFile(string value)
        {
            TargetFile = new FileInfo(value).Name;
        }

        public double Check(string source, string target)
        {
            Index = new Cosine().Similarity(source, target);
            return Index;
        }
    }
}
