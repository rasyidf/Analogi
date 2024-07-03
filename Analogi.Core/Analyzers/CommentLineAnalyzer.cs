using Analogi.Core.Interfaces;
using Analogi.Core.Models;
using Analogi.Core.Reasons;

namespace Analogi.Core.Analyzers
{
    public class CommentLineAnalyzer : IPipeline
    {
        public PipelineData Run(PipelineData data)
        {
            IdenticalLoCReason r = new()
            {
                IsComment = true
            };
            string a = data.FileMetadataMappings["file.1.comment"].Count.ToString();
            string b = data.FileMetadataMappings["file.2.comment"].Count.ToString();
            _ = r.Check(a, b);
            if (r.Index > r.Treshold)
            {
                data.AddReason(r, data.FileMetadataMappings["file.path"][1]);
            }
            return data;
        }
    }
}