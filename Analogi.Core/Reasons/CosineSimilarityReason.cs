using F23.StringSimilarity;

namespace Yaudah.Core
{
    public class CosineSimilarityReason : IReason
    {
        public string ReasonString => $"The Code have high similarity index with :{TargetFile}";

        public string TargetFile { get; private set; }

        public double Index { get; set; }
        public void SetTargetFile(string value)
        {
            TargetFile = value;
        }

        public double Check(string source, string target)
        {
            Index = new Cosine().Similarity(source, target);
            return Index;
        }
    }
}
