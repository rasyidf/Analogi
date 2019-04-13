namespace Analogi.Core.PostProcess
{
    public class Cleanup : IPipeline
    {
        public PipelineData Run(PipelineData data)
        {
            data.Metadatas.Clear();
            return data;
        }
    }
}