using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Analogi.Core.Extractors
{
    public class CodeExtractor : IExtractor
    {
        readonly Regex CommentRegex = new Regex("//[^\n]*\n?");

        readonly Regex CommentBlockRegex = new Regex(@"/\*[^*]*\*+(?:[^/*][^*]*\*+)*/");

        public void Run(ref PipelineData pd, string id, CodeFile data)
        {
            var file = data.ReadAll();
            string v = CommentBlockRegex.Replace(file, "");
             pd.AddMetadata(Name, id,  Run(v.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)));
        }

        public List<string> Run(IEnumerable<string> data)
        {
            var a = new List<string>();
            foreach (var item in data)
            {
                a.Add(CommentRegex.Replace(item, ""));
            }
            return a;
        }
        
        public string Name { get => "code"; }
    }
}
