using Analogi.Core.Interfaces;
using Analogi.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Analogi.Core.Extractors
{
    public partial class CommentExtractor : IExtractor
    {
        private readonly Regex CommentRegex = SingleLineCommentRegex();
        private readonly Regex CommentBlockRegex = MultiLineCommentRegex();

        public void Run(ref PipelineData pd, string id, CodeFile data)
        {
            string file = data.ReadAll();
            MatchCollection v = CommentBlockRegex.Matches(file);
            MatchCollection w = CommentRegex.Matches(file);

            pd.AddMetadata(Name, id, Run(v, w));

        }

        private static List<string> Run(MatchCollection v, MatchCollection w)
        {
            List<string> str = (from Match item in v
                                select item.Value).ToList();
            str.AddRange(from Match item in w
                         select item.Value);
            return str;
        }

        public string Name => "comment";

        [GeneratedRegex("//[^(\n|\r)]*\n?")]
        private static partial Regex SingleLineCommentRegex();

        [GeneratedRegex(@"/\*[^*]*\*+(?:[^/*][^*]*\*+)*/")]
        private static partial Regex MultiLineCommentRegex();
    }
}
