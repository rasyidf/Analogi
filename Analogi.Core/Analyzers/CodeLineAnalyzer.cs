using System.IO;

namespace rasyidf.Analogi.Core.Analyzers
{
    public class CodeLineAnalyzer : IPipeline
    {
        public PipelineData Run(PipelineData data)
        {
            var r = new IdenticalLoCReason();
            var a = data.Metadatas["file.1.code"].Count.ToString();
            var b = data.Metadatas["file.2.code"].Count.ToString();
            r.Check(a, b);
            if (r.Index > r.Treshold)
            {
                data.AddReason(r, data.Metadatas["file.path"][1]);
            }
            return data;
        }
    }
    public class IdenticalLoCReason : IReason {
        public double Bias { get; set; } = 1;
        public double Index { get; set; }
        public string ReasonString => $"The File LoC is Identical with {TargetFile}";

        public string TargetFile { get; private set; }
        public double Treshold => 0.5;

        public void SetTargetFile(string value)
        {
            TargetFile = new FileInfo(value).Name;
        }

        public double Check(string source, string target)
        {
            if (source == "0" || target == "0" )
            {
                return 0;
            }
            return (source == target) ? 1 : 0;
        }
    }
}