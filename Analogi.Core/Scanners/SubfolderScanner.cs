using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace rasyidf.Analogi.Core
{

    namespace Scanner
    {
        public class SubfolderScanner : IScanner
        {
            public string Path { get; set; }

            public SubfolderScanner(string path)
            {
                Path = path;
            }

            public IEnumerable<CodeFile> Scan()
            { 
                var a = new List<CodeFile>();
                foreach (var item in Directory.EnumerateDirectories(Path))
                {
                    a.Add(new CodeFile(item));
                }
                return a;

            }
        }
    }
}