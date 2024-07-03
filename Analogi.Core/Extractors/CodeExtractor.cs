using Analogi.Core.Interfaces;
using Analogi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Analogi.Core.Extractors
{
    public partial class CodeExtractor : IExtractor
    {
        private readonly Regex CommentRegex = SingleLineCommentRegex();
        private readonly Regex CommentBlockRegex = MultiLineCommentRegex();

        public void Run(ref PipelineData pd, string id, CodeFile data)
        {
            string file = data.ReadAll();
            string v = CommentBlockRegex.Replace(file, "");
            pd.AddMetadata(Name, id, Run(v.Split(separator, StringSplitOptions.RemoveEmptyEntries)));
        }

        public List<string> Run(IEnumerable<string> data)
        {
            List<string> a = [];
            a.AddRange(from string item in data
                       select CommentRegex.Replace(item, ""));
            return a;
        }

        public string Name => "code";

        private static readonly char[] separator = ['\n', '\r'];

        [GeneratedRegex("//[^\n]*\n?")]
        private static partial Regex SingleLineCommentRegex();
        [GeneratedRegex(@"/\*[^*]*\*+(?:[^/*][^*]*\*+)*/")]
        private static partial Regex MultiLineCommentRegex();
    }
}
