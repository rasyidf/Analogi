namespace Analogi.Core.Analyzers
{
    public class CommentLineAnalyzer : IPipeline
    {
        public PipelineData Run(PipelineData data)
        {
            var r = new IdenticalLoCReason
            {
                IsComment = true
            };
            var a = data.Metadatas["file.1.comment"].Count.ToString();
            var b = data.Metadatas["file.2.comment"].Count.ToString();
            r.Check(a, b);
            if (r.Index > r.Treshold)
            {
                data.AddReason(r, data.Metadatas["file.path"][1]);
            }
            return data;
        }
    }                                      
}      