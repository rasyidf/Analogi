using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace rasyidf.Analogi.Core.Extractors
{
    public class CodeExtractor : IExtractor
    {
        readonly Regex CommentRegex = new Regex("//[^\n]*\n?");

        readonly Regex CommentBlockRegex = new Regex(@"/\*[^*]*\*+(?:[^/*][^*]*\*+)*/");

        public List<string> Run(string data)
        {
            var file = System.IO.File.ReadAllText(data);
            string v = CommentBlockRegex.Replace(file, "");
            return Run(v.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
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
