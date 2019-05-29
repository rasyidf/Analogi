using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Analogi.Core.Extractors
{
    public class StructureExtractor : IExtractor
    {
        readonly Regex RoutineRegex = new Regex(@"(?<type>int|void|bool|float|double|string)\s+(?<id>[A-Za-z][A-Za-z0-0_]+)\((?<args>.*)\)\s*\{?", RegexOptions.Compiled);

        readonly Regex IncludeRegex = new Regex(@"#\s+include\s+<(?:[a-z][a-z]+)>");


        public void Run(ref PipelineData pd, string id, CodeFile data)
        {
            var file = data.ReadAll();
            int HeaderLen = 0, subroutineLen = 0;
            var d = RoutineRegex.Matches(file);
            foreach (Match i in d)
            {
                if (i.Groups["id"].Value.ToLower() == "main")
                {
                    HeaderLen = i.Index;
                }
                else
                {
                    if (HeaderLen == -1)
                    {
                        HeaderLen = i.Index;
                    }
                    else
                    {
                        if (HeaderLen > i.Index)
                        {

                            subroutineLen = HeaderLen;
                            HeaderLen = i.Index;
                        }
                        else
                            subroutineLen = i.Index;    
                    }
                }
            }
            if (HeaderLen > subroutineLen)
            {
                var tmp = HeaderLen;
                HeaderLen = subroutineLen;
                subroutineLen = tmp;
            }

            subroutineLen = subroutineLen - HeaderLen;

            pd.AddMetadata(Name, id + ".header", Run(file.Substring(0, HeaderLen).Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)));
            pd.AddMetadata(Name, id + ".main", Run(file.Substring(HeaderLen, subroutineLen).Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)));
            pd.AddMetadata(Name, id + ".subroutine", Run(file.Substring(subroutineLen).Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)));
        }
        public List<string> Run(IEnumerable<string> data)
        {
            var sb = new List<string>(data);
            return sb;
        }

        public string Name { get => "structure"; }
    }
}
