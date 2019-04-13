using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Analogi.Core
{

    namespace Scanner
    {
        public class FolderScanner : IScanner
        {
            public string Path { get; set; }

            public FolderScanner(string path)
            {
                Path = path;
            }

            public IEnumerable<CodeFile> Scan()
            {
                var a = new List<CodeFile>();
                foreach (var item in Directory.EnumerateFiles(Path, "*.*"))
                {
                    a.Add(new CodeFile(item));
                }
                return a;       
            }
        }
    }
}