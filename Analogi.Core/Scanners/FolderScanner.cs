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
                return (from item in Directory.EnumerateFiles(Path, "*.cpp")
                        select new CodeFile(item)).ToList();       
            }
        }
    }
}