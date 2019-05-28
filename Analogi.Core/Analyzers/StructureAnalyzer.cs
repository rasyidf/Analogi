namespace Analogi.Core.Analyzers
{
    public class StructureAnalyzer : IPipeline
    {

        public PipelineData Run(PipelineData data)
        {

            string[] regions =   { "header", "main", "subroutine" };
            foreach (var region in regions)
            {
                CheckSimilarity(ref data, region);    
            }

            return data;
        }

        private static void CheckSimilarity(ref PipelineData data, string region)
        {
            var r = new IdenticalStructureReason();
            var a = string.Join(" ", data.Metadatas["file.1." + region + ".structure"]);
            var b = string.Join(" ", data.Metadatas["file.2." + region + ".structure"]);

            r.Check(a, b);
            if (r.Index > r.Treshold)
            {
                r.Region = region;
                data.AddReason(r, data.Metadatas["file.path"][1]);
            }
        }
    }
}