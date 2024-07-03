using Analogi.Core.Interfaces;
using Analogi.Core.Models;
using Analogi.Core.Reasons;

namespace Analogi.Core.Analyzers
{
    public class CosineSimilarityAnalyzer : IPipeline
    {
        public PipelineData Run(PipelineData data)
        {
            CosineSimilarityReason r = new();
            string a = string.Join(" ", data.FileMetadataMappings["file.1.code"]);
            string b = string.Join(" ", data.FileMetadataMappings["file.2.code"]);
            _ = r.Check(a, b);
            if (r.Index > r.Treshold)
            {
                data.AddReason(r, data.FileMetadataMappings["file.path"][1]);
            }
            return data;
        }
    }
}