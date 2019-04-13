namespace Analogi.Core.Analyzers
{
    public class CodeLineAnalyzer : IPipeline
    {
        public PipelineData Run(PipelineData data)
        {
            var r = new IdenticalLoCReason
            {
                IsComment = false
            };
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
}