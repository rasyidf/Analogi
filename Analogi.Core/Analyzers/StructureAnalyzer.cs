using Analogi.Core.Interfaces;
using Analogi.Core.Models;
using Analogi.Core.Reasons;


namespace Analogi.Core.Analyzers
{
    public class StructureAnalyzer : IPipeline
    {
        public PipelineData Run(PipelineData data)
        {
            string[] regions = ["header", "main", "subroutine"];
            foreach (string region in regions)
            {
                CheckSimilarity(ref data, region);
            }

            return data;
        }

        private static void CheckSimilarity(ref PipelineData data, string region)
        {
            IdenticalStructureReason reason = new();
            string a = string.Join(" ", data.FileMetadataMappings["file.1." + region + ".structure"]);
            string b = string.Join(" ", data.FileMetadataMappings["file.2." + region + ".structure"]);

            _ = reason.Check(a, b);
            if (reason.Index > reason.Treshold)
            {
                reason.Region = region;
                data.AddReason(reason, data.FileMetadataMappings["file.path"][1]);
            }
        }
    }
}