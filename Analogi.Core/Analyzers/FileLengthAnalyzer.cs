namespace Analogi.Core.Analyzers
{
    public class FileLengthAnalyzer : IPipeline
    {
        public PipelineData Run(PipelineData data)
        {
            var r = new IdenticalSizeReason();
            var a =  new CodeFile(data.Metadatas["file.path"][0]);
            var b = new CodeFile(data.Metadatas["file.path"][1]);
            r.Check(a.GetFile(), b.GetFile());

            if (r.Index > r.Treshold)
            {
                data.AddReason(r, data.Metadatas["file.path"][1]);
            }
            return data;
        }
    }
}