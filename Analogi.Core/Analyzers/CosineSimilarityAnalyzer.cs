namespace Analogi.Core.Analyzers
{
    public class CosineSimilarityAnalyzer : IPipeline
    {
        public PipelineData Run(PipelineData data)
        {
            var r = new CosineSimilarityReason();
            var a = string.Join(" ", data.Metadatas["file.1.code"]);
            var b = string.Join(" ", data.Metadatas["file.2.code"]);
            r.Check(a,b);
            if (r.Index > r.Treshold)
            {
                data.AddReason(r, data.Metadatas["file.path"][1]);     
            }
            return data;                       
        }
    }
}