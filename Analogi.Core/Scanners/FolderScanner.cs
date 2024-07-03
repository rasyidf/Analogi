using Analogi.Core.Interfaces;
using Analogi.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Analogi.Core
{

    namespace Scanner
    {
        public class FolderScanner(string path) : IScanner
        {
            public string Path { get; set; } = path;

            public IEnumerable<CodeFile> Scan()
            {
                return (from item in Directory.EnumerateFiles(Path, "*.cpp")
                        select new CodeFile(item)).ToList();
            }
        }
    }
}