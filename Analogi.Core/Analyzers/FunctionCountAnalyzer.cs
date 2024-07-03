using Analogi.Core.Interfaces;
using Analogi.Core.Models;
using Analogi.Core.Reasons;


namespace Analogi.Core.Analyzers
{
    public class FunctionCountAnalyzer : IPipeline
    {
        public PipelineData Run(PipelineData data)
        {
            // methods regex but ignore main method
            IdenticalLoCReason r = new()
            {
                IsComment = false
            };
            string a = data.FileMetadataMappings["file.1.code"].Count.ToString();
            string b = data.FileMetadataMappings["file.2.code"].Count.ToString();
            _ = r.Check(a, b);
            if (r.Index > r.Treshold)
            {
                data.AddReason(r, data.FileMetadataMappings["file.path"][1]);
            }
            return data;
        }
    }
}