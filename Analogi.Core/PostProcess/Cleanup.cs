using Analogi.Core.Interfaces;
using Analogi.Core.Models;

namespace Analogi.Core.PostProcess
{
    public class Cleanup : IPipeline
    {
        public PipelineData Run(PipelineData data)
        {
            data.FileMetadataMappings.Clear();
            return data;
        }
    }
}