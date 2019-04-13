namespace Analogi.Core
{
    public interface IPipeline
    {
        PipelineData Run(PipelineData data);
    }
         
}