using Analogi.Core.Models;

namespace Analogi.Core.Interfaces
{
    public interface IPipeline
    {
        PipelineData Run(PipelineData data);
    }

}