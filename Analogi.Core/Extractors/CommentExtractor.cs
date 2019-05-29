using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Analogi.Core.Extractors
{
    public class CommentExtractor : IExtractor
    {
        readonly Regex CommentRegex = new Regex("//[^(\n|\r)]*\n?");

        readonly Regex CommentBlockRegex = new Regex(@"/\*[^*]*\*+(?:[^/*][^*]*\*+)*/");

         public void Run(ref PipelineData pd, string id, CodeFile data)
         {
            var file = data.ReadAll();
            var v = CommentBlockRegex.Matches(file);
            var w = CommentRegex.Matches(file);
            
            pd.AddMetadata(Name, id,  Run(v,w) ); 
             
        }

        private List<string> Run(MatchCollection v, MatchCollection w)
        {
            var str = (from Match item in v
                       select item.Value).ToList();
            str.AddRange(from Match item in w
                         select item.Value);
            return str;
        }

        public string Name { get => "comment"; } 
    }
}
