using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Analogi.Core
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
                return (from item in Directory.EnumerateDirectories(Path)
                        select new CodeFile(item)).ToList();     
            }
        }
    }
}