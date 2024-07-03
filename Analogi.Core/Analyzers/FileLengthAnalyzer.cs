using Analogi.Core.Interfaces;
using Analogi.Core.Models;
using Analogi.Core.Reasons;

namespace Analogi.Core.Analyzers
{
    public class FileLengthAnalyzer : IPipeline
    {
        public PipelineData Run(PipelineData data)
        {
            IdenticalSizeReason r = new();
            CodeFile a = new(data.FileMetadataMappings["file.path"][0]);
            CodeFile b = new(data.FileMetadataMappings["file.path"][1]);
            _ = r.Check(a.GetFile(), b.GetFile());

            if (r.Index > r.Treshold)
            {
                data.AddReason(r, data.FileMetadataMappings["file.path"][1]);
            }
            return data;
        }
    }
}