using System.Collections.Generic;
using System.IO;

namespace rasyidf.Analogi.Core
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

            public IEnumerable<string> Scan()
            {
                return Directory.EnumerateFiles(Path, "*.*");       
            }
        }
    }
}